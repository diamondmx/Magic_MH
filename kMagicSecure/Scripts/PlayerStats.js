function SetPlayerNameDropdown(name)
{
	var playerDropdown = document.getElementById("playerName");
	var playerName = name;

	for (var i = 0; i < playerDropdown.options.length; i++) {
		if (playerDropdown[i].value == playerName) {
			playerDropdown[i].selected = true;
		}
	}
}

