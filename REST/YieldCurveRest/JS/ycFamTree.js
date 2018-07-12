

function buildTree() {

	var ycDef = getYcDef();
	var ccy = getCcy();
	var ycFam = getYcFam();

	var outStr = "<ul>";

	for (var i = 0; i < ccy.ccyList.length; i++) {

		var c = ccy.ccyList[i];

		var isCcyAdded = false;

		for (var k = 0; k < ycFam.ycFamList.length; k++) {

			var f = ycFam.ycFamList[k];
			if (c.Id != f.CurrencyId)
				continue;

			for (var j = 0; j < ycDef.ycDefList.length; j++) {

				var d = ycDef.ycDefList[j];
				if (d.FamilyId != f.Id || d.CurrencyId != c.Id)
					continue;

				if (!isCcyAdded) {
					outStr += "<li><input type=\"checkbox\" id=\"" + c.Code + "\" /><label for=\"" + c.Code + "\">" + c.Code + "</label><ul>";
					isCcyAdded = true;
				}
				outStr += "<li><a href=\"#\" onclick=\"drawChart('20-08-2013', '" + d.Id + "');drawGridEntry('20-08-2013', '" + d.Id + "');\">" 
			    		+ d.Name + "</a></li>";
				///outStr += "<li><a href=\"#\" onclick=\"drawChart('20-08-2013', '" + d.Id + "');\">" + d.Name + "</a></li>";

				//outStr += "<li><a href=\"./MainPage.html\" onClick=\"drawDashboard('20-08-2013', '" + d.Id + "')\" >" + d.Name + "</a></li>";
				//outStr += "<li><a href=\"./YieldCurve.html\" onClick=\"drawChart('20-08-2013', '" + d.Id + "')\" >" + d.Name + "</a></li>";
			}
		}

		if (isCcyAdded)
			outStr += "</ul></li>";
	}

	outStr += "</ul>";

	return outStr;
}

function getYcDef() {

	var jsonYcDef = $.ajax({
		url: "../YieldCurveApi.svc/yc/def",
		dataType: "json",
		async: false
	}).responseText;

	var ycDef = JSON.parse(jsonYcDef);
	return ycDef;
}

function getCcy() {

	var jsonCcy = $.ajax({
		url: "../YieldCurveApi.svc/yc/ccy",
		dataType: "json",
		async: false
	}).responseText;

	var ccy = JSON.parse(jsonCcy);
	return ccy;
}

function getYcFam() {

	var jsonYcFam = $.ajax({
		url: "../YieldCurveApi.svc/yc/fam",
		dataType: "json",
		async: false
	}).responseText;

	var ycFam = JSON.parse(jsonYcFam);
	return ycFam;
}