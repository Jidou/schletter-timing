import { saveAs } from 'file-saver';
import moment from 'moment';

let blobStream = require('blob-stream');
let pdf = require('pdfjs');
let fonts = require('pdfjs/font/Helvetica');
// let boldFonts = require('pdfjs/font/HelveticaBold');


export function calcuateTimeDiffs(groups) {
    let minTime;

    groups.forEach(group => {
        let tmp = moment(group.timeTaken);

        if (!minTime) {
            minTime = tmp;
        } else if (minTime.isAfter(tmp)) {
            minTime = tmp;
        }
    });

    groups.forEach(group => {
        let tmp = moment(group.timeTaken);
        var tmp2 = tmp
            .subtract(minTime.format("SSS"), 'ms')
            .subtract(minTime.format("ss"), 's')
            .subtract(minTime.format("mm"), 'm')
            .subtract(minTime.format("HH"), 'h');
        group.timeDiff = tmp2;
    });
}


export function generatePdf(race, groups, logo) {
    var doc = new pdf.Document({ font: fonts.Helvetica })

    addHeader(doc, race, logo);
    addFooter(doc);

    doc.cell({ paddingBottom: 0.5 * pdf.cm });
    doc.cell({ font: fonts.HelveticaBold, fontSize: 16, textAlign: 'left' }).text('Gesamtwertung');
    doc.cell({ paddingBottom: 0.5 * pdf.cm });

    addTable(doc, groups);

    download(doc, race.titel)

    doc.end();
}


export function generatePdfWithFilter(race, groups, filter, filterValues, logo) {
    var doc = new pdf.Document({ font: fonts.Helvetica })

    addHeader(doc, race, logo);
    addFooter(doc);

    doc.cell({ paddingBottom: 0.5 * pdf.cm })

    filter(groups, filterValues)

    filterValues.forEach(value => {
        let filteredGroups = filter(groups, value);

        if (filteredGroups.length > 0) {
            doc.cell({ font: fonts.HelveticaBold, fontSize: 16, textAlign: 'left' }).text(value.cn);
            doc.cell({ paddingBottom: 0.5 * pdf.cm });
            calcuateTimeDiffs(filteredGroups);
            addTable(doc, filteredGroups);
            doc.cell({ paddingBottom: 0.5 * pdf.cm });
        }

    });

    download(doc, race.titel)

    doc.end();
}


export function generatePdfWithCustomResult(race, groups, customResultTitle, logo) {
    var doc = new pdf.Document({ font: fonts.Helvetica })

    addHeader(doc, race, logo);
    addFooter(doc);

    doc.cell({ paddingBottom: 0.5 * pdf.cm });
    doc.cell({ font: fonts.HelveticaBold, fontSize: 16, textAlign: 'left' }).text(customResultTitle);
    doc.cell({ paddingBottom: 0.5 * pdf.cm });

    calcuateTimeDiffs(groups);

    addTable(doc, groups);

    download(doc, race.titel)

    doc.end();
}


export function test () {
    // var FileSaver = require('file-saver');

    var doc = new pdf.Document({ font: fonts.Helvetica })

    var header = doc.header();
    var headerTable = header.table({ widths: [null] })
    headerTable.row().cell().text({ textAlign: 'center', fontSize: 14, font: fonts.HelveticaBold }).add('SchletterTrophy - Summer Challenge 2019')
    headerTable.row().cell().text({ textAlign: 'center', paddingBottom: 0.5 * pdf.cm }).add('Lauf- Rad Kombination\nOffizielle Ergebnissliste')

    var today = new Date("2020-01-15");
    var da = new Intl.DateTimeFormat('de', { year: 'numeric' }).format(today);
    var mo = new Intl.DateTimeFormat('de', { month: 'short' }).format(today);
    var ye = new Intl.DateTimeFormat('de', { day: '2-digit' }).format(today);

    var footer = doc.footer();
    var footerRow = footer.table({ widths: [null, null, null] }).row();
    footerRow.cell({ width: 33 }).text({ textAlign: 'left' })
        .add(`${da}-${mo}-${ye}`);
    footerRow.cell({ width: 33 }).text({ textAlign: 'center' })
        .add('Auswertung: SV Sellrain\nTiming: Alge Timing');
    // footerRow.cell({ width: 33 }).text({ textAlign: 'right' }).add(function (curr, total) { return curr + ' / ' + total });
    footer.pageNumber(function (curr, total) { return curr + ' / ' + total }, { textAlign: 'right' });

    var table = doc.table({
        widths: [1.5 * pdf.cm, 1.5 * pdf.cm, 2 * pdf.cm, 2 * pdf.cm, null, 2 * pdf.cm, 2 * pdf.cm],
        borderHorizontalWidths: function (i) { return i < 2 ? 1 : 0.1 },
        padding: 5
    })

    var tr = table.header({ font: fonts.HelveticaBold })
    tr.cell('Rang')
    tr.cell('Stnr')
    tr.cell('Gruppenname')
    tr.cell('Kategorie')
    tr.cell('Teilnehmer')
    tr.cell('Total')
    tr.cell('Diff')

    addRow(table, "1", "3", 'Article A', "lorem", "asdf asdf", "fdsa fdsa", "00:10:15.1235", "-" )
    // addRow(1, 'Article B', lorem, 250)
    // addRow(2, 'Article C', lorem, 330)
    // addRow(3, 'Article D', lorem, 1220)
    // addRow(2, 'Article E', lorem, 120)
    // addRow(5, 'Article F', lorem, 50)

    download(doc, "test")

    doc.end();
}


function addHeader(doc, race, logo) {
    if (!logo || logo.length <= 0) {
        var header = doc.header();
        var headerTable = header.table({ widths: [null] });
        headerTable.row().cell({ font: fonts.HelveticaBold }).text({ textAlign: 'center', fontSize: 21 }).add(race.titel);
        headerTable.row().cell().text({ textAlign: 'center' }).add(race.raceType);
        headerTable.row().cell().text({ textAlign: 'center' }).add("Offizielle Ergebnissliste");
    } else {
        var img = new pdf.Image(byteToUint8Array(logo));
        var header = doc.header();
        var headerTable = header.table({ widths: [null, null] });
        var row = headerTable.row();
        row.cell({ font: fonts.HelveticaBold }).text({ textAlign: 'center', fontSize: 21 }).add(race.titel);
        row.cell().image(img, { height: 2 * pdf.cm });
        headerTable.row().cell().text({ textAlign: 'center' }).add(race.raceType);
        headerTable.row().cell().text({ textAlign: 'center' }).add("Offizielle Ergebnissliste");
    }
}


function byteToUint8Array(byteArray) {
    var uint8Array = new ArrayBuffer(byteArray.length);
    for (var i = 0; i < uint8Array.length; i++) {
        uint8Array[i] = byteArray[i];
    }

    return uint8Array;
}


function addFooter(doc) {
    var today = new Date();
    var da = new Intl.DateTimeFormat('de', { year: 'numeric' }).format(today);
    var mo = new Intl.DateTimeFormat('de', { month: 'short' }).format(today);
    var ye = new Intl.DateTimeFormat('de', { day: '2-digit' }).format(today);

    var footer = doc.footer();
    var footerRow = footer.table({ widths: [null, null, null] }).row();
    footerRow.cell({ width: 33 }).text({ textAlign: 'left' })
        .add(`${da}-${mo}-${ye}`);
    footerRow.cell({ width: 33 }).text({ textAlign: 'center' })
        .add('Auswertung: SV Sellrain\nTiming: Alge Timing');
    footer.pageNumber(function (curr, total) { return curr + ' / ' + total }, { textAlign: 'right' });
}


function addTable(doc, groups) {
    var table = doc.table({
        widths: [1.5 * pdf.cm, 1.3 * pdf.cm, 4.5 * pdf.cm, 2.5 * pdf.cm, 4.5 * pdf.cm, 2.75 * pdf.cm, 2.75 * pdf.cm],
        borderHorizontalWidths: function (i) { return i < 2 ? 1 : 0.1 },
        padding: 5,
    });

    var tr = table.header({ font: fonts.HelveticaBold, fontSize: 13 });
    tr.cell('Rang', { font: fonts.HelveticaBold });
    tr.cell('Stnr', { font: fonts.HelveticaBold });
    tr.cell('Gruppenname', { font: fonts.HelveticaBold });
    tr.cell('Kategorie', { font: fonts.HelveticaBold });
    tr.cell('Teilnehmer', { font: fonts.HelveticaBold });
    tr.cell('Total', { font: fonts.HelveticaBold });
    tr.cell('Diff', { font: fonts.HelveticaBold });

    var realRank = 1;

    for (let rank = 0; rank < groups.length; rank++) {
        const group = groups[rank];
        addRow(table, realRank, group.startnumber, group.groupname, group.participant1Category, group.participant2Category, group.participant1Name, group.participant2Name, group.timeTaken, group.timeDiff);
        realRank++;
    }
}


function addRow(table, rank, startNr, groupname, categorie1, categorie2, participant1, participant2, total, diff) {
    var tr = table.row();
    tr.cell(rank.toString());
    tr.cell(startNr.toString());
    tr.cell(groupname);
    tr.cell(categorie1 + '\n' + categorie2);
    tr.cell(participant1 + '\n' + participant2);
    tr.cell(total.format('HH:mm:ss.SSS'));
    tr.cell(diff.format('HH:mm:ss.SSS'));
}


function download(doc, racetile) {
    const stream = doc.pipe(blobStream());

    // doc.end();
    stream.on('finish', function () {
        // get a blob you can do whatever you like with
        // const blob = stream.toBlob('application/pdf');

        // or get a blob URL for display in the browser
        const url = stream.toBlobURL('application/pdf');
        saveAs(url, racetile + ".pdf");
        // download('test.pdf', blob);
    });
}
