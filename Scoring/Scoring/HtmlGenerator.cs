using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVelocity.App;
using NVelocity;
using System.Windows.Forms;
using System.IO;

namespace Scoring
{
    public class HtmlGenerator
    {
        private static VelocityEngine velocity;
        private static VelocityContext baseContext;
        
        static HtmlGenerator()
        {
            velocity = new VelocityEngine();
            var p = new Commons.Collections.ExtendedProperties();

#if DEBUG
            string temp = Application.ExecutablePath;
            string[] split = Application.ExecutablePath.Split('\\');
            string pathRoot = string.Join("\\", split, 0, split.Length - 3);
            //cssFile = string.Format(@"file:///{0}\html\best.css", pathRoot);
            //scriptFile = string.Format(@"file:///{0}\html\scripts.js", pathRoot);
            p.AddProperty("file.resource.loader.path", new System.Collections.ArrayList(new string[] { pathRoot }));
#endif
            velocity.Init(p);
            baseContext = new VelocityContext();
            baseContext.Put("script", "scripts.js");
            baseContext.Put("css", "best.css");
        }

        public enum PageId
        {
            RoundFull,
            RoundPartial,
            LastRound,
            OverallScores,
            OverallScoresSplit,
            TeamScores,
            TeamRounds,
            ScoringReferee,
            ScoringMaster,
            ScoringBlank,
        }

        private static Dictionary<PageId, string> fileMap = new Dictionary<PageId, string>
        {
            { PageId.RoundFull, "round_display_full.html" },
            { PageId.RoundPartial, "round_display.html" },
            { PageId.LastRound, "last_round.html" },
            { PageId.OverallScores, "overall_scores_full.html" },
            { PageId.OverallScoresSplit, "overall_scores_{0}.html" },
            { PageId.TeamScores, "team_{0}.html" },
            { PageId.TeamRounds, "team_{0}.html" },
            { PageId.ScoringReferee, "ref_field_{0}.html" },
            { PageId.ScoringMaster, "ref_master.html" },
            { PageId.ScoringBlank, "ref_master_blank.html" },
        };

        public static string GetPageId(PageId id) { return fileMap[id]; }

        public static void RoundDisplay(PageId type, string title, IEnumerable<Round> rounds)
        {
            if (type != PageId.RoundFull && type != PageId.RoundPartial)
                throw new ArgumentException("invalid PageId");

            Template t = velocity.GetTemplate(@"templates\round_display.vm");
            VelocityContext c = new VelocityContext(baseContext);
            c.Put("rounds", rounds);
            c.Put("title", title);
            StringWriter sw = new StringWriter();
            t.Merge(c, sw);

            string fileName = fileMap[type];            
            File.WriteAllText("html\\" + fileName, sw.ToString());
        }

        public static void LastRoundDisplay(Round current, Round next1, Round next2)
        {
            Template template = velocity.GetTemplate(@"templates\last_round_scores.vm");
            VelocityContext c = new VelocityContext(baseContext);

            if (current != null)
            {
                var scores = from t in current.Teams select new { Name = t.Name, Score = t.GetScore(current), TotalScore = t.TotalScore(current.Type) };
                c.Put("current", current);
                c.Put("scores", scores);
            }
            else
            {
                List<object> scores = new List<object>();
                scores.Add(new object());
                scores.Add(scores[0]);
                scores.Add(scores[0]);
                scores.Add(scores[0]);
                c.Put("scores", scores);
            }

            System.Collections.ArrayList list = new System.Collections.ArrayList();
            if (next1 != null)
            {
                list.Add(new { Number = next1.Number, Red = next1.Red.Name, Green = next1.Green.Name, Blue = next1.Blue.Name, Yellow = next1.Yellow.Name });
            }
            else
            {
                list.Add(new object());
            }

            if (next2 != null)
            {
                list.Add(new { Number = next2.Number, Red = next2.Red.Name, Green = next2.Green.Name, Blue = next2.Blue.Name, Yellow = next2.Yellow.Name });
            }
            else
            {
                list.Add(new object());
            }

            c.Put("next", list);

            StringWriter sw = new StringWriter();
            template.Merge(c, sw);

            string fileName = fileMap[PageId.LastRound];
            File.WriteAllText("html\\" + fileName, sw.ToString());
        }

        [System.Diagnostics.DebuggerDisplay("{Name} = {Place} with {Score}")]
        private class TempScore
        {
            public string Name { get; set; }
            public int Place { get; set; }
            public int Score { get; set; }
        }

        private static IEnumerable<TempScore> PlaceTeams(IEnumerable<TempScore> scores)
        {
            int place = 1;
            int count = 1;
            double last = double.MaxValue;

            var s = scores.ToList();

            foreach (var t in s)
            {
                if (t.Score == last)
                {
                    t.Place = place;
                }
                else
                {
                    t.Place = count;
                    place = count;
                    last = t.Score;
                }

                ++count;
            }

            return s;
        }

        public static void OverallScoresDisplay(IEnumerable<Team> teams, Round.Types type)
        {
            Template template = velocity.GetTemplate(@"templates\overall_scores.vm");

            //filter teams by round
            var teamSet = teams.Where(t => t.Rounds.Any(r => r.Type == type));

            var scores = (from t in teamSet orderby t.TotalScore(type) descending select new TempScore { Name = t.Name, Score = (int)t.TotalScore(type) }).ToList();

            PlaceTeams(scores);

            string path = "html\\" + fileMap[PageId.OverallScoresSplit];

            int total = scores.Count;
            for (int i = 0; i < total; i += 16)
            {
                VelocityContext c = new VelocityContext(baseContext);
                var section = scores.Skip(i).Take(16).ToList();
                c.Put("teams", section);
                StringWriter sw = new StringWriter();
                template.Merge(c, sw);

                File.WriteAllText(string.Format(path, i / 16), sw.ToString());
            }
            //}
            //else
            //{
            //    VelocityContext c = new VelocityContext(baseContext);
            //    c.Put("teams", scores);
            //    StringWriter sw = new StringWriter();
            //    template.Merge(c, sw);
            //    File.WriteAllText("html\\" + fileMap[PageId.OverallScores], sw.ToString());
            //}
        }

        public static void OverallScoresDisplayPrint(List<Team> teams)
        {
            List<Round.Types> rType = new List<Round.Types> { Round.Types.Preliminary, Round.Types.Wildcard, Round.Types.Semifinals, Round.Types.Finals };
            
            //1 group teams by what round they competed                       
            Dictionary<Round.Types, List<TempScore>> groups = new Dictionary<Round.Types, List<TempScore>>();
            foreach (var type in rType)
            {
                var scores = teams.Where(t => t.Rounds.Any(r => r.Type == type)).OrderByDescending(t => t.TotalScore(type)).Select(t => new TempScore { Score = (int)t.TotalScore(type), Name = t.Name, Place = 0 });
                scores = PlaceTeams(scores);
                groups.Add(type, scores.ToList());
            }

            Template template = velocity.GetTemplate(@"templates\overall_scores_print.vm");
            VelocityContext c = new VelocityContext(baseContext);
            c.Put("scores", groups);
            StringWriter sw = new StringWriter();
            template.Merge(c, sw);
            File.WriteAllText("html\\" + fileMap[PageId.OverallScores], sw.ToString());
        }

        public static void TeamRoundsDisplay(List<Team> teams)
        {
            Directory.CreateDirectory(@"html\schedules");

            Template template = velocity.GetTemplate(@"templates\team_rounds_display.vm");
            VelocityContext c = new VelocityContext(baseContext);
            
            foreach (var t in teams)
            {
                c.Put("name", t.Name);
                var roundGroup = from r in t.Rounds group r by r.Type into s select new { Type = s.Key, Rounds = from r1 in s select new { r1.Number, Color = r1.TeamColor(t) } };                
                c.Put("roundGroups", roundGroup);
                StringWriter sw = new StringWriter();
                template.Merge(c, sw);
                File.WriteAllText(@"html\schedules\" + string.Format(fileMap[PageId.TeamRounds], t.Number), sw.ToString());
            }
        }

        public static void TeamScoreDisplay(List<Team> teams)
        {
            Directory.CreateDirectory(@"html\scores");

            Template template = velocity.GetTemplate(@"templates\team_scores_display.vm");
            VelocityContext c = new VelocityContext(baseContext);

            foreach (var t in teams)
            {
                c.Put("title", t.Name);
                
                var scoreGroups = from s in t.Scores group s by s.Round.Type into score select new { Type = score.Key, Scores = score };
                c.Put("scores",scoreGroups);
                
                StringWriter sw = new StringWriter();
                template.Merge(c, sw);
                File.WriteAllText(@"html\scores\" + string.Format(fileMap[PageId.TeamScores], t.Number), sw.ToString());
            }            
        }

        public static void RefereeFieldSheets(IEnumerable<Round> rounds)
        {
            Template t = velocity.GetTemplate(@"templates\ref_field_sheet.vm");
            VelocityContext c = new VelocityContext(baseContext);

            int[] colors = new int[] { 0, 1, 2, 3 };
            
            foreach (var color in colors)
            {
                c.Put("color", Round.TeamColor(color));
                var teams = from r in rounds select new { Number = r.Number, TeamName = r.Teams[color].Name };
                c.Put("rounds", teams);

                StringWriter sw = new StringWriter();
                t.Merge(c, sw);
                File.WriteAllText("html\\" + string.Format(fileMap[PageId.ScoringReferee], Round.TeamColor(color).ToLower()), sw.ToString());
            }
        }

        public static void RefereeMasterSheets(IEnumerable<Round> rounds)
        {
            Template template = velocity.GetTemplate(@"templates\ref_master_sheet.vm");
            VelocityContext c = new VelocityContext(baseContext);

            var rnds = from r in rounds orderby r.Number select new { Number = r.Number, Teams = from t in r.Teams select new { Name = t.Name, Color = r.TeamColor(t) } };
            c.Put("rounds", rnds);

            StringWriter sw = new StringWriter();
            template.Merge(c, sw);
            File.WriteAllText("html\\" + fileMap[PageId.ScoringMaster], sw.ToString());
        }

        //public static void RefereeMasterSheets(IEnumerable<Round> rounds)
        //{
        //    Template template = velocity.GetTemplate(@"templates\ref_master_sheet.vm");
        //    VelocityContext c = new VelocityContext(baseContext);

        //    var rnds = from r in rounds orderby r.Number select new { Number = r.Number, Teams = from t in r.Teams select new { Name = t.Name, Color = r.TeamColor(t) } };
        //    c.Put("rounds", rnds);

        //    StringWriter sw = new StringWriter();
        //    template.Merge(c, sw);
        //    File.WriteAllText("html\\" + fileMap[PageId.ScoringMaster], sw.ToString());
        //}
    }
}
