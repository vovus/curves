

function drawChart(date, idYc) {

	var urlStr = "../YieldCurveApi.svc/yc/ephc/" + date + "/" + idYc;
    //var urlStr = "../YieldCurveSrv.svc/yc/ephc/20-08-2013/3";
	/*
	var jsonData = $.ajax({
		url: urlStr,
		dataType: "json",
		async: false
	}).responseText;

	var options = {
		title: 'Entry Points'
	};
	*/
	//jsonData = "{\"cols\": [{\"type\": \"string\" ,\"id\": \"Term\" ,\"label\": \"Term\" }, {\"type\": \"number\" ,\"id\": \"Rate\" ,\"label\": \"Rate\" }], \"rows\" : [{\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.00581227623}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.00585089849}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.005937955}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.00623064917}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.0070035}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.00828948168}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.00885972828}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.01006488648}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.01121775732}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.0119927376}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.0127630425}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.01352715}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.01435403125}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.01519737375}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.01570139104}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.01570905}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.01989108}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.023590016}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.026716778}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.029058372}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.031163116}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.032807664}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.03401531}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.03574124}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.036982946}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.038630428}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.039593026}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.039742872}]}, {\"c\" : [{\"v\": \"05-00-2011\"}, {\"v\": 0.040107964}]}]}";

	// Create our data table out of JSON data loaded from server.
	//var data = new google.visualization.DataTable(jsonData);

	//var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
    //chart.draw(data, options);

    /*
    var dt = new dataTable({
        "bProcessing": true,
        "sAjaxSource": jsonData
        });
    */

    //jsonData.parse();
    /*
    var sampleData = [];
    for (var i = 0, len = jsonData.length; i < len; i++) {
        var result = jsonData[i];
        sampleData.push({ result.duration, result.rate });
    }
    */

    /*
    var g = new Dygraph(
        // containing div
        document.getElementById("chart_div"),

        // CSV or path to a CSV file.
        [
                [1,0.00581227623],
                [7, 0.00585089849],
                [14, 0.005937955]
              ],
              {
                labels: [ "x", "A" ]
              }
    );
    */

	//$.getJSON('../YieldCurveSrv.svc/yc/ephc/20-08-2013/3', function (data) {
	$.getJSON(urlStr, function (data) {

		var items = [];
		var termFormatted = [];

		$.each(data, function (key, val) {

			var pair = [];
			pair.push(val['Duration']);
			pair.push(val['Rate'] * 100);

			items.push(pair);

			// array of pairs, e.g. [7, '7d'], [1, '1y']
			var pairFormatted = [];
			pairFormatted.push(val['Duration']);
			pairFormatted.push(val['Term']);
			termFormatted.push(pairFormatted);
		})

		g = new Dygraph(document.getElementById("ycChart"),
                        items,
                        {
                        	title: 'YieldCurve',
                        	titleHeight: 20,
                        	xlabel: 'Term',
                        	ylabel: 'Rate,%',
                        	drawPoints: true,
                        	pointSize: 3,
                        	highlightCircleSize: 5,
                        	legend: 'always',
                        	//includeZero: true,

                        	labels: ["Term", "Rate,%"],
                        	digitsAfterDecimal: 2,

                        	axisLabelWidth: 50,
                        	xlabel: 'Term',
                        	//axisTickSize: 40,

                        	axes: {
                        		x: {
                        			//axisTickSize: 40,
                        			pixelsPerLabel: 100,

                        			ticker: function (a, b, pixels, opts, dygraph, vals) {
                        				vals = getXticks(a, b, pixels, opts);
                        				return Dygraph.numericTicks(a, b, pixels, opts, dygraph, vals)
                        			},
                        			valueFormatter: function (val, opts, series_name, dygraph) {
                        				return formatX(val);
                        			},
                        			axisLabelFormatter: function (val, granularity, opts, dygraph) {
                        				return formatX(val);
                        			}
                        		}
                        	},

                        	// range slider
                        	showRangeSelector: true,
                        	rangeSelectorHeight: 30,
                        	//rangeSelectorPlotStrokeColor: 'green',
                        	//rangeSelectorPlotFillColor: 'lightyellow',
                        	rollPeriod: 1
                        	//showRoller: true,
                        	//customBars: true,
                        });

		function formatX(v) {

			for (var i = 0; i < termFormatted.length; i++) {
				if (termFormatted[i][0] == v) {
					return termFormatted[i][1];
				}
			}
			return '';
		}

		function getXticks(min, max, pixels, opts) {

			//var pixels_between_ticks = opts('axisTickSize');
			var pixels_per_tick = opts('pixelsPerLabel');
			var nTicks = Math.floor(pixels / pixels_per_tick);
			var values_per_tick = Math.floor(max / nTicks);

			var vals = [];

			var last_inserted_val = 0;
			for (var i = termFormatted.length - 1; i >= 0; i--) {

				if (termFormatted[i][0] <= max
					&& ((last_inserted_val - termFormatted[i][0]) >= values_per_tick 
						|| last_inserted_val == 0)
					) {
					vals.push(termFormatted[i][0]);
					//nTicks--;
					last_inserted_val = termFormatted[i][0];
				}
			}
			return vals.reverse();
		}

	});
}
