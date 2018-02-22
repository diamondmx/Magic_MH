function insertNewMatch(event) {
	var matchLocator = "#" + event.target.id;
	var player1 = $(matchLocator + "A")[0].value;
	var player2 = $(matchLocator + "B")[0].value;
	var eventName = GLOBALS.EventName;
	var round = GLOBALS.Round;

	$.post("/Magic/AdminInsertMatch",
		{
			player1: player1,
			player2: player2,
			eventName: eventName,
			round: round
		},
		function (data) { // Success
			location.reload();
		}).fail(function (data) { // Handle Error

		});
}

function playerNameChange(event) {
	var playerIDLocator = "#" + event.target.id + ".playerID";
	var buttonLocatorID = event.target.id.slice(0, -1);
	var modifyButtonLocator = "#" + buttonLocatorID + ".ModifyMatchButton";
	var insertButtonLocator = "#" + buttonLocatorID + ".InsertNewMatch";
	$(playerIDLocator)[0].value = findIDByName(event.target.value);

	var theButton = $(modifyButtonLocator)[0];
	if (!theButton) {
		theButton = $(insertButtonLocator)[0];
	}
	
	theButton.removeAttribute('disabled');

}

$(".playerName").change(playerNameChange);

$("#dialog-confirm").dialog({
	resizable: false,
	height: "auto",
	width: 400,
	modal: true,
	autoOpen: false,
	buttons: {}
});

$(".ModifyMatchButton").click(function (event) {
	event.target.setAttribute("disabled", "disabled");

	var matchLocator = '#' + event.target.id;
	var player1 = $(matchLocator + "A")[0].value;
	var player2 = $(matchLocator + "B")[0].value;
	var oldPlayer1 = $(matchLocator + "C")[0].value;
	var oldPlayer2 = $(matchLocator + "D")[0].value;
	var eventName = GLOBALS.EventName;
	var round = GLOBALS.Round;

	$.post("/Magic/AdminModifyMatch",
		{
			player1: player1,
			player2: player2,
			oldPlayer1: oldPlayer1,
			oldPlayer2: oldPlayer2,
			eventName: eventName,
			round: round
		},
		function (data) { // Success
			location.reload();
		}).fail(function (data) { // Handle Error

		});
});

$(".DeleteMatchButton").click(function (event) {
	var matchLocator = "#" + event.target.id;
	var oldPlayer1 = $(matchLocator + "C")[0].value;
	var oldPlayer2 = $(matchLocator + "D")[0].value;
	var eventName = GLOBALS.EventName;
	var round = GLOBALS.Round;

	$.post("/Magic/AdminDeleteMatch",
		{
			player1: oldPlayer1,
			player2: oldPlayer2,
			eventName: eventName,
			round: round
		},
		function (data) { // Success
			location.reload();
		}).fail(function (data) { // Handle Error

		});
})
$(".AddMatchButton").click(function (event) {
	var newID = findMaxID();
	createMatchTableRow(newID+1);
});

$(".DeleteAllButton").click(function (event) {
	var actionURL = "/Magic/GetMatchCountInRound?eventName=" + GLOBALS.EventName + "&Round=" + GLOBALS.Round;
	var jsonResult = $.getJSON(actionURL, displayDeleteAllPrompt);
})

function displayDeleteAllPrompt(response) {
	if (response != null) {
		var deletePrompt = "Delete " + response + " items";
		var dlgbuttons = {};
		dlgbuttons[deletePrompt] = function () {
			$(this).dialog("close");
			var eventName = GLOBALS.EventName;
			var round = GLOBALS.Round;
			var actionURL = "/Magic/AdminDeleteAllMatchesInRound"

			$.post(actionURL,
				{
					eventName: eventName,
					round: round
				},
				function (response) {
					location.reload();
			});
		};
		dlgbuttons["Cancel"] = function () {
			$(this).dialog("close");
			alert("CANCELLED");
		};

		$("#dialog-confirm").dialog({
			resizable: false,
			height: "auto",
			width: 400,
			modal: true,
			autoOpen: true,
			buttons: dlgbuttons
		})
	}
}

function findIDByName(name) {
	for (i = 0; i < GLOBALS.PlayerIDList.length; i++) {
		if (GLOBALS.PlayerNameList[i] === name) {
			return GLOBALS.PlayerIDList[i];
		}
	}
} 

function findMaxID() {
	var max = -999;
	$(".ModifyMatchButton").each(function (a, b) { max = Math.max(max, b.id); });
	return max;
}

function createMatchTableRow(newID) {
	var p1IdElement = document.createElement("INPUT");
	p1IdElement.setAttribute("type", "text");
	p1IdElement.setAttribute("value", "NEW");
	p1IdElement.setAttribute("id", newID + "A");
	p1IdElement.setAttribute("disabled", "disabled");
	p1IdElement.setAttribute("class", "playerID")
	var p1IdDatum = document.createElement("TD");
	p1IdDatum.appendChild(p1IdElement);

	var p1NameElement = document.createElement("INPUT");
	p1NameElement.setAttribute("type", "text");
	p1NameElement.setAttribute("value", "NEW");
	p1NameElement.setAttribute("list", "PlayerNames");
	p1NameElement.setAttribute("id", newID + "A");
	p1NameElement.addEventListener("change", playerNameChange);
	var p1NameDatum = document.createElement("TD");
	p1NameDatum.appendChild(p1NameElement);

	var p2IdElement = document.createElement("INPUT");
	p2IdElement.setAttribute("type", "text");
	p2IdElement.setAttribute("value", "NEW");
	p2IdElement.setAttribute("id", newID + "B");
	p2IdElement.setAttribute("class", "playerID")
	p2IdElement.setAttribute("disabled", "disabled");
	var p2IdDatum = document.createElement("TD");
	p2IdDatum.appendChild(p2IdElement);

	var p2NameElement = document.createElement("INPUT");
	p2NameElement.setAttribute("type", "text");
	p2NameElement.setAttribute("value", "NEW");
	p2NameElement.setAttribute("id", newID + "B");
	p2NameElement.setAttribute("list", "PlayerNames");
	p2NameElement.addEventListener("change", playerNameChange);
	var p2NameDatum = document.createElement("TD");
	p2NameDatum.appendChild(p2NameElement);

	var scoreElement = document.createElement("INPUT");
	scoreElement.setAttribute("type", "text");
	scoreElement.setAttribute("value", "NEW");
	scoreElement.setAttribute("id", newID);
	scoreElement.setAttribute("disabled", "disabled");
	var scoreDatum = document.createElement("TD");
	scoreDatum.appendChild(scoreElement);

	var buttonElement = document.createElement("BUTTON");
	buttonElement.setAttribute("id", newID);
	buttonElement.setAttribute("class", "InsertNewMatch");
	buttonElement.innerText = "Insert";
	var buttonDatum = document.createElement("TD");
	buttonElement.addEventListener("click", insertNewMatch);
	buttonDatum.appendChild(buttonElement);
	

	var matchRow = document.createElement("TR")
	matchRow.appendChild(p1IdDatum);
	matchRow.appendChild(p1NameDatum);
	matchRow.appendChild(p2IdDatum);
	matchRow.appendChild(p2NameDatum)
	matchRow.appendChild(scoreDatum);
	matchRow.appendChild(buttonDatum);

	$(".ButtonRow")[0].before(matchRow);

}

