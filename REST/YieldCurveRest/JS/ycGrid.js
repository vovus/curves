/*
 *	expect in a format:

 {
  "page": "1",			// page number in reply (start with 1 = default value)
  "total": "2",			// total number of pages 
  "records": "10",		// total number of records (e.g. records <= page * total)
						// rows itself with ID of each row (internal) comming 1st)
  "rows": [
      {
          "id": 3,
          "cell": [
              3,
              "cell 1",
              "2010-09-29T19:05:32",
              "2010-09-29T20:15:56",
              "hurrf",
              0 
          ] 
      },
      {
          "id": 1,
          "cell": [
              1,
              "teaasdfasdf",
              "2010-09-28T21:49:21",
              "2010-09-28T21:49:21",
              "aefasdfsadf",
              1 
          ] 
      } 
  ]
}

 */


function drawGridEntry(date, idYc) {

	var urlStr = "../YieldCurveApi.svc/yc/ephg/" + date + "/" + idYc;
	if (typeof (date) === "undefined" || typeof (idYc) === "undefined")
		urlStr = "../YieldCurveApi.svc/yc/ephg/-1/-1";

	//var urlStr = "../YieldCurveApi.svc/yc/ephg/" + date + "/" + idYc;
	//$.getJSON(urlStr, function (data) {
	var jsonData = $.ajax({

		url: urlStr,
		datatype: "json",
		//mtype: "GET",
		colNames: ["Term", "Rate", "Type", "Reference"],
		colModel: [
			    { name: "Term", width: 100 },
			    { name: "Rate", width: 100, align: "left" },
			    { name: "Type", width: 100, align: "left" },
				{ name: "Reference", width: 300, align: "left" }
		    ],
		//pager: "#pager",
		//rowNum: 10,
		//rowList: [10, 20, 30],
		sortname: "Term",
		sortorder: "desc",
		viewrecords: true,
		gridview: true,
		autoencode: true,
		caption: "Entry Points"
	}).responseText;

		$("#entry").jqGrid({
			url: urlStr,
			datatype: "json",
			//mtype: "GET",
			colNames: ["Term", "Rate", "Type", "Reference"],
			colModel: [
			    { name: "Term", width: 100 },
			    { name: "Rate", width: 100, align: "left" },
			    { name: "Type", width: 100, align: "left" },
				{ name: "Reference", width: 300, align: "left" }
		    ],
			//pager: "#pager",
			//rowNum: 10,
			//rowList: [10, 20, 30],
			sortname: "Term",
			sortorder: "desc",
			viewrecords: true,
			gridview: true,
			autoencode: true,
			caption: "Entry Points"
		});
	//});
}

//yc/disc/{date}/{id}/{epl}/{ddl}
function drawGridZC(date, idYc) {

	var urlStr = "../YieldCurveApi.svc/yc/disc/" + date + "/" + idYc + "/" + "/";

	$("#zc").jqGrid({
		url: "",
		datatype: "json",
		mtype: "POST",
		colNames: ["Date", "ZC Rate", "Frw Rate", "Discount"],
		colModel: [
            { name: "date", width: 100 },
            { name: "zcrate", width: 100 },
            { name: "frwrate", width: 100, align: "right" },
            { name: "dis", width: 100, align: "right" }
        ],
		pager: "#pager",
		//rowNum: 10,
		//rowList: [10, 20, 30],
		sortname: "date",
		sortorder: "desc",
		viewrecords: true,
		gridview: true,
		autoencode: true,
		caption: "Discounts"
	});
}

