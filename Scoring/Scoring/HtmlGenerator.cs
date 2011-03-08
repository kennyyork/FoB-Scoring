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
        }

        private static Dictionary<PageId, string> fileMap = new Dictionary<PageId, string>
        {
            { PageId.RoundFull, "round_display_full.html" },
            { PageId.RoundPartial, "round_display.html" },
            { PageId.LastRound, "last_round.html" },
            { PageId.OverallScores, "overall_scores.html" },
            { PageId.OverallScoresSplit, "overall_scores_{0}.html" },
            { PageId.TeamScores, "team_{0}.html" },
            { PageId.TeamRounds, "team_{0}.html" },
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

        private class TempScore
        {
            public string Name { get; set; }
            public int Place { get; set; }
            public double Score { get; set; }
        }

        public static void OverallScoresDisplay(IEnumerable<Team> teams, Round.Types type, bool split)
        {
            Template template = velocity.GetTemplate(@"templates\overall_scores.vm");

            //filter teams by round
            var teamSet = teams.Where(t => t.Rounds.Any(r => r.Type == type));

            var scores = (from t in teamSet orderby t.TotalScore(type) descending select new TempScore { Name = t.Name, Score = t.TotalScore(type) }).ToList();

            int place = 1;
            int count = 1;
            double last = double.MaxValue;

            foreach (var t in scores)
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

            if (split)
            {
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
            }
            else
            {
                VelocityContext c = new VelocityContext(baseContext);
                c.Put("teams", scores);
                StringWriter sw = new StringWriter();
                template.Merge(c, sw);
                File.WriteAllText("html\\" + fileMap[PageId.OverallScores], sw.ToString());
            }
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
                File.WriteAllText( @"html\schedules\" + string.Format(fileMap[PageId.TeamRounds], t.Number),sw.ToString());
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
    }
}
