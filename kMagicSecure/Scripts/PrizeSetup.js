function addRow(eventName, round) {

	var lastPosition = parseInt($("#PrizeTableBody tr:last").find("input[name='Position']").prop('value'));
	var lastPrize = parseInt($("#PrizeTableBody tr:last").find("input[name='Packs']").prop('value'));
	
	if(lastPosition === null || isNaN(lastPosition))
	{
		lastPosition = 0;
	}

	if (lastPrize === null || isNaN(lastPrize))
	{
		lastPrize = "";
	}

	var newPosition = lastPosition + 1;
	var newPrize = lastPrize;
		
	var eventElement = document.createElement("INPUT");
	eventElement.setAttribute("type", "hidden");
	eventElement.setAttribute("value", eventName);
	eventElement.setAttribute("name", "EventName");
	var eventDatum = document.createElement("TD")
	eventDatum.appendChild(eventElement);

	var roundElement = document.createElement("INPUT");
	roundElement.setAttribute("type", "hidden");
	roundElement.setAttribute("value", round);
	roundElement.setAttribute("name", "Round");
	var roundDatum = document.createElement("TD")
	roundDatum.appendChild(roundElement);

	var positionElement = document.createElement("INPUT");
	positionElement.setAttribute("type", "text");
	positionElement.setAttribute("value", newPosition);
	positionElement.setAttribute("name", "Position");
	var positionDatum = document.createElement("TD")
	positionDatum.appendChild(positionElement);

	var packsElement = document.createElement("INPUT");
	packsElement.setAttribute("type", "text");
	packsElement.setAttribute("value", newPrize);
	packsElement.setAttribute("name", "Packs");
	var packsDatum = document.createElement("TD")
	packsDatum.appendChild(packsElement);

	var otherElement = document.createElement("INPUT");
	otherElement.setAttribute("type", "text");
	otherElement.setAttribute("value", "");
	otherElement.setAttribute("name", "Other");
	var otherDatum = document.createElement("TD")
	otherDatum.appendChild(otherElement);

	var prizeRow = document.createElement("TR")
	prizeRow.appendChild(positionDatum);
	prizeRow.appendChild(packsDatum);
	prizeRow.appendChild(otherDatum);
	prizeRow.appendChild(eventDatum)
	prizeRow.appendChild(roundDatum)

	$("#PrizeTableBody").append(prizeRow);
}

function removeLastRow() {
	$("#PrizeTableBody tr:last").remove();
}