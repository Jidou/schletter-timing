using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPdf;
using Model;
using NLog;

namespace PdfGenerator {
    public class PdfRenderer {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();


        public static void GroupByClassAndOrder(Race race) {
            try {
                var renderer = new HtmlToPdf();
                AddFooter(renderer, race);
                var html = $"{BuildDocHeaders(race)}{GroupByClassOrderAndGenerateHtml(race)}";
                var PDF = renderer.RenderHtmlAsPdf(html);
                var OutputPath = "Data/ResultByClass.pdf";
                PDF.SaveAs(OutputPath);
            } catch (Exception ex) {
                logger.Error(ex, "Error during creation of pdf");
            }
        }


        public static void CreateStartList(Race race) {
            try {
                var renderer = new HtmlToPdf();
                AddFooter(renderer, race);
                var html = $"{BuildDocHeaders(race)}{GenerateStartListHtml(race)}";
                var PDF = renderer.RenderHtmlAsPdf(html);
                var OutputPath = "Data/StartList.pdf";
                PDF.SaveAs(OutputPath);
            } catch (Exception ex) {
                logger.Error(ex, "Error during creation of pdf");
            }
        }


        private static object GenerateStartListHtml(Race race) {
            var orderedByGroupNumber = race.Participants.OrderBy(x => x.Groupnumber);
            var body = string.Empty;
            AddTableHeadersStartList(ref body);

            foreach (var group in orderedByGroupNumber) {
                AddTableRowStartList(group, ref body);
            }

            body += "</table>";

            return body;
        }


        public static void Order(Race race) {
            try {
                var renderer = new HtmlToPdf();
                AddFooter(renderer, race);
                var html = $"{BuildDocHeaders(race)}{OrderAndGenerateHtml(race)}";
                var PDF = renderer.RenderHtmlAsPdf(html);
                var OutputPath = "Data/Result.pdf";
                PDF.SaveAs(OutputPath);
            } catch (Exception ex) {
                logger.Error(ex, "Error during creation of pdf");
            }
        }


        private static string OrderAndGenerateHtml(Race race) {
            var body = string.Empty;

            var orderedByRank = race.Participants.OrderBy(x => x.FinishTime);
            AddTableHeaders(ref body);

            var first = orderedByRank.First();
            int rank = 1;

            foreach (var group in orderedByRank) {
                AddTableRow(group, first, rank, ref body);
                rank++;
            }

            body += "</table>";

            return body;
        }


        private static string GroupByClassOrderAndGenerateHtml(Race race) {
            var groupedByClass = race.Participants.GroupBy(x => x.Class);
            var body = string.Empty;

            foreach (var @class in groupedByClass) {
                var orderedByRank = @class.OrderBy(x => x.FinishTime);
                body += $"<h2>{@class.Key}</h2><br>";
                AddTableHeaders(ref body);

                var first = orderedByRank.First();
                int rank = 1;

                foreach (var group in orderedByRank) {
                    AddTableRow(group, first, rank, ref body);
                    rank++;
                }

                body += "</table>";
            }

            return body;
        }


        private static string BuildDocHeaders(Race race) {
            return $@"
<center>
    <text>
        <font size=""6"">
            <b>{race.Titel}</b>
        </font>
        <br>
        <font size=""4"">{race.RaceType}</font>
        <br>
        <font size=""4"">Offizielle Ergebnissliste</font>
    </text>
</center>
<hr/>";

//            return $@"
//<center>
//    <h1>{race.Titel}</h1>
//    <p>{race.RaceType}</p>
//    <p>Offizielle Ergebnissliste</p>
//</center>
//<hr/>";
        }


        private static void AddTableHeaders(ref string body) {
            body += $@"<table style=""width:100%"">
 <tr>
    <th>Rang</th>
    <th>Stnr</th> 
    <th>Gruppenname</th>
    <th>Kategorie</th>
    <th>Teilnehmer</th>
    <th>Total</th>
    <th>Diff</th>
</tr>
";
        }


        private static void AddTableHeadersStartList(ref string body) {
            body += $@"<table style=""width:100%"">
 <tr>
    <th>Stnr</th> 
    <th>Gruppenname</th>
    <th>Kategorie</th>
    <th>Teilnehmer</th>
</tr>
";
        }


        private static void AddTableRowStartList(Group group, ref string body) {
            body += $@"<tr>
    <td><b>{group.Groupnumber}<b></td> 
    <td>{group.Groupname}</td>
    <td>{group.Participant1.Category} <br> {group.Participant2.Category}</td>
    <td>{group.Participant1.Firstname} {group.Participant1.Lastname} <br> {group.Participant2.Firstname} {group.Participant2.Lastname}</td>
</tr>";
        }


        private static void AddTableRow(Group group, Group first, int rank, ref string body) {
            body += $@"<tr>
    <td><b>{rank}<b></td>
    <td><b>{group.Groupnumber}<b></td> 
    <td>{group.Groupname}</td>
    <td>{group.Participant1.Category} <br> {group.Participant2.Category}</td>
    <td>{group.Participant1.Firstname} {group.Participant1.Lastname} <br> {group.Participant2.Firstname} {group.Participant2.Lastname}</td>
    <td>{group.TimeTaken.ToString(@"hh\:mm\:ss\.FFF")}</td>
    <td>{(group.TimeTaken - first.TimeTaken).ToString(@"hh\:mm\:ss\.FFF")}</td>
</tr>";
        }


        private static void AddFooter(HtmlToPdf renderer, Race race) {
            renderer.PrintOptions.Footer = new SimpleHeaderFooter() {
                LeftText = $"{race.Date.ToString("d.M.yyyy")}",
                CenterText = $"Auswertung: SV Sellrain\nTiming:{race.TimingTool.ToString()}",
                RightText = "Seite {page}/{total-pages}",
                DrawDividerLine = false,
                FontSize = 11
            };
        }
    }
}