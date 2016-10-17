// Main script

if (typeof jQuery == 'undefined')
{
	alert("jQuery not loaded");
}		

var matchDlg = $("#matchDialog");
matchDlg.dialog(
	{
		autoOpen: false,
		position: {
			my: "center",
			at: "center",
			of: $("body"),
			within: $("body")
		}});

setupAllMatchPopups();

function setupAllMatchPopups() {
	var allBtns = $(".matchPopupLink");

	allBtns.click(function (event) {
		var target = $(event.target);
		var p1 = target.attr("data-player");
		var p2 = target.attr("data-opponent");
		var r = target.attr("data-round");
		var e = target.attr("data-event");

		var url = "/Magic/Match/" + e + "/" + r + "/" + p1 + "/" + p2 + "/";

		matchDlg.load(encodeURI(url) + " #matchForm");
		matchDlg.dialog("open");

	})
}