// Main script

if (typeof jQuery == 'undefined')
{
	alert("jQuery not loaded");
}

var matchDlg = $("#matchDialog");
var dialogButton = $("#button");
setupDialog(matchDlg, dialogButton);



function setupDialog(dlg, btn) {
	dlg.dialog({ autoOpen: false });

	btn.click(function () {
		dlg.load("/Magic/Match/OGW/1/TESTPLAYER1/TESTPLAYER2")
		dlg.dialog("open");
	})
	return dlg;
}