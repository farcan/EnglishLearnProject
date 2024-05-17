import pdfMake from "pdfmake/build/pdfmake";
import pdfFonts from "pdfmake/build/vfs_fonts";
pdfMake.vfs = pdfFonts.pdfMake.vfs;


var data = document.getElementById("table-div");


function indir() {
    pdfMake.createPdf(data).download();

}
