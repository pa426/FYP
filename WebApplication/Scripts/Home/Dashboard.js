$(function() {

    'use strict';

    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */

    //-----------------------
    //- MONTHLY SALES CHART -
    //-----------------------

    var groups = new Array();
    for (var i = 0; i < datax.$values.length; i++) {
        if (datax.$values[i].GroupName == '   ') {
            groups.push('OtherVideos');
        } else {
            groups.push(datax.$values[i].GroupName);
        }
        
    }
    var uniqueGroups = groups.filter(function (item, pos) {
         return groups.indexOf(item) == pos;
    });

    console.log(uniqueGroups);
    var listOfValues = new Array();
    var listOfNames = new Array();

    for (var i = 0; i < uniqueGroups.length; i++) {
        var z = 1;
        var listVideosForGroup = new Array();
        var listNamesForGroup = new Array();
        for (var j = 0; j < datax.$values.length; j++) {
            if (datax.$values[j].GroupName == uniqueGroups[i]) {
                listVideosForGroup.push(datax.$values[j].MainSentiment);
                listNamesForGroup.push('Video ' + [z]);
                z++;
            } else if (datax.$values[j].GroupName == '   ' && uniqueGroups[i] == 'OtherVideos') {
                listVideosForGroup.push(datax.$values[j].MainSentiment);
                listNamesForGroup.push('Video ' + [z]);
                z++;
            }
            
        }
        listOfValues.push(listVideosForGroup);
        listOfNames.push(listNamesForGroup);
    }

    console.log(listOfValues);

    var salesChartOptions = {
        scaleLabel: function (valuePayload) {
            if(Number(valuePayload.value)===0)    
                return 'Negative';
            if(Number(valuePayload.value)===1)    
                return 'Neutral';
            if(Number(valuePayload.value)===2)    
                return 'Positive';
        },
        //Boolean - If we should show the scale at all
        showScale: true,
        //Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: false,
        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0,0,0,.05)",
        //Number - Width of the grid lines
        scaleGridLineWidth: 1,
        //Boolean - Whether to show horizontal lines (except X axis)
        scaleShowHorizontalLines: true,
        //Boolean - Whether to show vertical lines (except Y axis)
        scaleShowVerticalLines: true,
        //Boolean - Whether the line is curved between points
        bezierCurve: true,
        //Number - Tension of the bezier curve between points
        bezierCurveTension: 0.3,
        //Boolean - Whether to show a dot for each point
        pointDot: false,
        //Number - Radius of each point dot in pixels
        pointDotRadius: 4,
        //Number - Pixel width of point dot stroke
        pointDotStrokeWidth: 1,
        //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
        pointHitDetectionRadius: 20,
        //Boolean - Whether to show a stroke for datasets
        datasetStroke: true,
        //Number - Pixel width of dataset stroke
        datasetStrokeWidth: 2,
        //Boolean - Whether to fill the dataset with a color
        datasetFill: true,
        //String - A legend template
        legendTemplate:
            "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%=datasets[i].label%></li><%}%></ul>",
        //Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
        maintainAspectRatio: true,
        //Boolean - whether to make the chart responsive to window resizing
        responsive: true

    };

    var canvasArray = [];
    var dataArray = [];

    for (var i = 0; i < uniqueGroups.length; i++) {

        document.getElementById("chartContainer").innerHTML += "<h3><center>" + uniqueGroups[i] + " Group Statistics </center></h3>";
        document.getElementById("chartContainer").innerHTML += "<canvas id=\"" + uniqueGroups[i] + "\" style='height: 140px;'></canvas>";
        var ctxPrep = "#" + uniqueGroups[i];
        canvasArray.push(ctxPrep);

        var colour = "#000000".replace(/0/g, function () { return (~~(Math.random() * 16)).toString(16); });

        var salesChartData = {
          
            labels: listOfNames[i],
            datasets: [
                {
                    fillColor: colour,
                    strokeColor: colour,
                    pointColor: colour,
                    pointStrokeColor: colour,
                    pointHighlightFill: colour,
                    pointHighlightStroke: colour,
                    data: listOfValues[i]
                }
            ]
        };

        dataArray.push(salesChartData);

    }

    console.log(dataArray);

    $.each(canvasArray, function (index, value) {
        var ctx = $(value).get(0).getContext("2d");
        var myNewChart = new Chart(ctx).Line(dataArray[index], salesChartOptions);
    });




    //---------------------------
    //- END MONTHLY SALES CHART -
    //---------------------------


    var countries = new Array();
    for (var i = 0; i < datax.$values.length; i++) {
        countries.push(datax.$values[i].VideoLocation);
    }
    var uniqueCountries = countries.filter(function (item, pos) {
        return countries.indexOf(item) == pos;
    });

    var markers = [
        { latLng: [34.51666667, 69.183333], name: "AF" },
        { latLng: [60.116667, 19.9], name: "AX" },
        { latLng: [41.31666667, 19.816667], name: "AL" },
        { latLng: [36.75, 3.05], name: "DZ" },
        { latLng: [-14.26666667, -170.7], name: "AS" },
        { latLng: [42.5, 1.516667], name: "AD" },
        { latLng: [-8.833333333, 13.216667], name: "AO" },
        { latLng: [18.21666667, -63.05], name: "AI" },
        { latLng: [0, 0], name: "AQ" },
        { latLng: [17.11666667, -61.85], name: "AG" },
        { latLng: [-34.58333333, -58.666667], name: "AR" },
        { latLng: [40.16666667, 44.5], name: "AM" },
        { latLng: [12.51666667, -70.033333], name: "AW" },
        { latLng: [-35.26666667, 149.133333], name: "AU" },
        { latLng: [48.2, 16.366667], name: "AT" },
        { latLng: [40.38333333, 49.866667], name: "AZ" },
        { latLng: [25.08333333, -77.35], name: "BS" },
        { latLng: [26.23333333, 50.566667], name: "BH" },
        { latLng: [23.71666667, 90.4], name: "BD" },
        { latLng: [13.1, -59.616667], name: "BB" },
        { latLng: [53.9, 27.566667], name: "BY" },
        { latLng: [50.83333333, 4.333333], name: "BE" },
        { latLng: [17.25, -88.766667], name: "BZ" },
        { latLng: [6.483333333, 2.616667], name: "BJ" },
        { latLng: [32.28333333, -64.783333], name: "BM" },
        { latLng: [27.46666667, 89.633333], name: "BT" },
        { latLng: [-16.5, -68.15], name: "BO" },
        { latLng: [43.86666667, 18.416667], name: "BA" },
        { latLng: [-24.63333333, 25.9], name: "BW" },
        { latLng: [-15.78333333, -47.916667], name: "BR" },
        { latLng: [-7.3, 72.4], name: "IO" },
        { latLng: [18.41666667, -64.616667], name: "VG" },
        { latLng: [4.883333333, 114.933333], name: "BN" },
        { latLng: [42.68333333, 23.316667], name: "BG" },
        { latLng: [12.36666667, -1.516667], name: "BF" },
        { latLng: [-3.366666667, 29.35], name: "BI" },
        { latLng: [11.55, 104.916667], name: "KH" },
        { latLng: [3.866666667, 11.516667], name: "CM" },
        { latLng: [45.41666667, -75.7], name: "CA" },
        { latLng: [14.91666667, -23.516667], name: "CV" },
        { latLng: [19.3, -81.383333], name: "KY" },
        { latLng: [4.366666667, 18.583333], name: "CF" },
        { latLng: [12.1, 15.033333], name: "TD" },
        { latLng: [-33.45, -70.666667], name: "CL" },
        { latLng: [39.91666667, 116.383333], name: "CN" },
        { latLng: [-10.41666667, 105.716667], name: "CX" },
        { latLng: [-12.16666667, 96.833333], name: "CC" },
        { latLng: [4.6, -74.083333], name: "CO" },
        { latLng: [-11.7, 43.233333], name: "KM" },
        { latLng: [-21.2, -159.766667], name: "CK" },
        { latLng: [9.933333333, -84.083333], name: "CR" },
        { latLng: [6.816666667, -5.266667], name: "CI" },
        { latLng: [45.8, 16], name: "HR" },
        { latLng: [23.11666667, -82.35], name: "CU" },
        { latLng: [12.1, -68.916667], name: "CW" },
        { latLng: [35.16666667, 33.366667], name: "CY" },
        { latLng: [50.08333333, 14.466667], name: "CZ" },
        { latLng: [-4.316666667, 15.3], name: "CD" },
        { latLng: [55.66666667, 12.583333], name: "DK" },
        { latLng: [11.58333333, 43.15], name: "DJ" },
        { latLng: [15.3, -61.4], name: "DM" },
        { latLng: [18.46666667, -69.9], name: "DO" },
        { latLng: [-0.216666667, -78.5], name: "EC" },
        { latLng: [30.05, 31.25], name: "EG" },
        { latLng: [13.7, -89.2], name: "SV" },
        { latLng: [3.75, 8.783333], name: "GQ" },
        { latLng: [15.33333333, 38.933333], name: "ER" },
        { latLng: [59.43333333, 24.716667], name: "EE" },
        { latLng: [9.033333333, 38.7], name: "ET" },
        { latLng: [-51.7, -57.85], name: "FK" },
        { latLng: [62, -6.766667], name: "FO" },
        { latLng: [6.916666667, 158.15], name: "FM" },
        { latLng: [-18.13333333, 178.416667], name: "FJ" },
        { latLng: [60.16666667, 24.933333], name: "FI" },
        { latLng: [48.86666667, 2.333333], name: "FR" },
        { latLng: [-17.53333333, -149.566667], name: "PF" },
        { latLng: [-49.35, 70.216667], name: "TF" },
        { latLng: [0.383333333, 9.45], name: "GA" },
        { latLng: [41.68333333, 44.833333], name: "GE" },
        { latLng: [52.51666667, 13.4], name: "DE" },
        { latLng: [5.55, -0.216667], name: "GH" },
        { latLng: [36.13333333, -5.35], name: "GI" },
        { latLng: [37.98333333, 23.733333], name: "GR" },
        { latLng: [64.18333333, -51.75], name: "GL" },
        { latLng: [12.05, -61.75], name: "GD" },
        { latLng: [13.46666667, 144.733333], name: "GU" },
        { latLng: [14.61666667, -90.516667], name: "GT" },
        { latLng: [49.45, -2.533333], name: "GG" },
        { latLng: [9.5, -13.7], name: "GN" },
        { latLng: [11.85, -15.583333], name: "GW" },
        { latLng: [6.8, -58.15], name: "GY	" },
        { latLng: [18.53333333, -72.333333], name: "HT" },
        { latLng: [0, 0], name: "HM" },
        { latLng: [14.1, -87.216667], name: "HN" },
        { latLng: [0, 0], name: "HK" },
        { latLng: [47.5, 19.083333], name: "HU" },
        { latLng: [64.15, -21.95], name: "IS" },
        { latLng: [28.6, 77.2], name: "IN" },
        { latLng: [-6.166666667, 106.816667], name: "ID" },
        { latLng: [35.7, 51.416667], name: "IR" },
        { latLng: [33.33333333, 44.4], name: "IQ" },
        { latLng: [53.31666667, -6.233333], name: "IE" },
        { latLng: [54.15, -4.483333], name: "IM" },
        { latLng: [31.76666667, 35.233333], name: "IL" },
        { latLng: [41.9, 12.483333], name: "IT" },
        { latLng: [18, -76.8], name: "JM" },
        { latLng: [35.68333333, 139.75], name: "JP" },
        { latLng: [49.18333333, -2.1], name: "JE" },
        { latLng: [31.95, 35.933333], name: "JO" },
        { latLng: [51.16666667, 71.416667], name: "KZ" },
        { latLng: [-1.283333333, 36.816667], name: "KE" },
        { latLng: [-0.883333333, 169.533333], name: "KI" },
        { latLng: [42.66666667, 21.166667], name: "KO" },
        { latLng: [29.36666667, 47.966667], name: "KW" },
        { latLng: [42.86666667, 74.6], name: "KG" },
        { latLng: [17.96666667, 102.6], name: "LA" },
        { latLng: [56.95, 24.1], name: "LV" },
        { latLng: [33.86666667, 35.5], name: "LB" },
        { latLng: [-29.31666667, 27.483333], name: "LS" },
        { latLng: [6.3, -10.8], name: "LR" },
        { latLng: [32.88333333, 13.166667], name: "LY" },
        { latLng: [47.13333333, 9.516667], name: "LI" },
        { latLng: [54.68333333, 25.316667], name: "LT" },
        { latLng: [49.6, 6.116667], name: "LU" },
        { latLng: [0, 0], name: "MO" },
        { latLng: [42, 21.433333], name: "MK" },
        { latLng: [-18.91666667, 47.516667], name: "MG" },
        { latLng: [-13.96666667, 33.783333], name: "MW" },
        { latLng: [3.166666667, 101.7], name: "MY" },
        { latLng: [4.166666667, 73.5], name: "MV" },
        { latLng: [12.65, -8], name: "ML" },
        { latLng: [35.88333333, 14.5], name: "MT" },
        { latLng: [7.1, 171.383333], name: "MH" },
        { latLng: [18.06666667, -15.966667], name: "MR" },
        { latLng: [-20.15, 57.483333], name: "MU" },
        { latLng: [19.43333333, -99.133333], name: "MX" },
        { latLng: [47, 28.85], name: "MD" },
        { latLng: [43.73333333, 7.416667], name: "MC" },
        { latLng: [47.91666667, 106.916667], name: "MN" },
        { latLng: [42.43333333, 19.266667], name: "ME" },
        { latLng: [16.7, -62.216667], name: "MS" },
        { latLng: [34.01666667, -6.816667], name: "MA" },
        { latLng: [-25.95, 32.583333], name: "MZ" },
        { latLng: [16.8, 96.15], name: "MM" },
        { latLng: [-22.56666667, 17.083333], name: "NA" },
        { latLng: [-0.5477, 166.920867], name: "NR" },
        { latLng: [27.71666667, 85.316667], name: "NP" },
        { latLng: [52.35, 4.916667], name: "NL" },
        { latLng: [-22.26666667, 166.45], name: "NC" },
        { latLng: [-41.3, 174.783333], name: "NZ" },
        { latLng: [12.13333333, -86.25], name: "NI" },
        { latLng: [13.51666667, 2.116667], name: "NE" },
        { latLng: [9.083333333, 7.533333], name: "NG" },
        { latLng: [-19.01666667, -169.916667], name: "NU" },
        { latLng: [-29.05, 167.966667], name: "NF" },
        { latLng: [39.01666667, 125.75], name: "KP" },
        { latLng: [15.2, 145.75], name: "MP	" },
        { latLng: [59.91666667, 10.75], name: "NO" },
        { latLng: [23.61666667, 58.583333], name: "OM" },
        { latLng: [33.68333333, 73.05], name: "PK" },
        { latLng: [7.483333333, 134.633333], name: "PW" },
        { latLng: [31.76666667, 35.233333], name: "PS" },
        { latLng: [8.966666667, -79.533333], name: "PA" },
        { latLng: [-9.45, 147.183333], name: "PG" },
        { latLng: [-25.26666667, -57.666667], name: "PY" },
        { latLng: [-12.05, -77.05], name: "PE" },
        { latLng: [14.6, 120.966667], name: "PH" },
        { latLng: [-25.06666667, -130.083333], name: "PN" },
        { latLng: [52.25, 21], name: "PL" },
        { latLng: [38.71666667, -9.133333], name: "PT" },
        { latLng: [18.46666667, -66.116667], name: "PR" },
        { latLng: [25.28333333, 51.533333], name: "QA" },
        { latLng: [-4.25, 15.283333], name: "CG" },
        { latLng: [44.43333333, 26.1], name: "RO" },
        { latLng: [55.75, 37.6], name: "RU" },
        { latLng: [-1.95, 30.05], name: "RW" },
        { latLng: [17.88333333, -62.85], name: "BL" },
        { latLng: [-15.93333333, -5.716667], name: "SH" },
        { latLng: [17.3, -62.716667], name: "KN" },
        { latLng: [14, -61], name: "LC" },
        { latLng: [18.0731, -63.0822], name: "MF" },
        { latLng: [46.76666667, -56.183333], name: "PM" },
        { latLng: [13.13333333, -61.216667], name: "VC" },
        { latLng: [-13.81666667, -171.766667], name: "WS" },
        { latLng: [43.93333333, 12.416667], name: "SM" },
        { latLng: [0.333333333, 6.733333], name: "ST" },
        { latLng: [24.65, 46.7], name: "SA" },
        { latLng: [14.73333333, -17.633333], name: "SN" },
        { latLng: [44.83333333, 20.5], name: "RS" },
        { latLng: [-4.616666667, 55.45], name: "SC" },
        { latLng: [8.483333333, -13.233333], name: "SL" },
        { latLng: [1.283333333, 103.85], name: "SG" },
        { latLng: [18.01666667, -63.033333], name: "SX" },
        { latLng: [48.15, 17.116667], name: "SK" },
        { latLng: [46.05, 14.516667], name: "SI" },
        { latLng: [-9.433333333, 159.95], name: "SB" },
        { latLng: [2.066666667, 45.333333], name: "SO" },
        { latLng: [-25.7, 28.216667], name: "ZA" },
        { latLng: [-54.283333, -36.5], name: "GS" },
        { latLng: [37.55, 126.983333], name: "KR" },
        { latLng: [4.85, 31.616667], name: "SS" },
        { latLng: [40.4, -3.683333], name: "ES" },
        { latLng: [6.916666667, 79.833333], name: "LK" },
        { latLng: [15.6, 32.533333], name: "SD" },
        { latLng: [5.833333333, -55.166667], name: "SR" },
        { latLng: [78.21666667, 15.633333], name: "SJ" },
        { latLng: [-26.31666667, 31.133333], name: "SZ" },
        { latLng: [59.33333333, 18.05], name: "SE" },
        { latLng: [46.91666667, 7.466667], name: "CH" },
        { latLng: [33.5, 36.3], name: "SY" },
        { latLng: [25.03333333, 121.516667], name: "TW" },
        { latLng: [38.55, 68.766667], name: "TJ" },
        { latLng: [-6.8, 39.283333], name: "TZ" },
        { latLng: [13.75, 100.516667], name: "TH" },
        { latLng: [13.45, -16.566667], name: "GM" },
        { latLng: [-8.583333333, 125.6], name: "TL" },
        { latLng: [6.116666667, 1.216667], name: "TG" },
        { latLng: [-9.166667, -171.833333], name: "TK" },
        { latLng: [-21.13333333, -175.2], name: "TO" },
        { latLng: [10.65, -61.516667], name: "TT" },
        { latLng: [36.8, 10.183333], name: "TN" },
        { latLng: [39.93333333, 32.866667], name: "TR" },
        { latLng: [37.95, 58.383333], name: "TM" },
        { latLng: [21.46666667, -71.133333], name: "TC" },
        { latLng: [-8.516666667, 179.216667], name: "TV" },
        { latLng: [0.316666667, 32.55], name: "UG" },
        { latLng: [50.43333333, 30.516667], name: "UA" },
        { latLng: [24.46666667, 54.366667], name: "AE" },
        { latLng: [51.5, -0.083333], name: "UK" },
        { latLng: [38.883333, -77], name: "US" },
        { latLng: [-34.85, -56.166667], name: "UY" },
        { latLng: [38.883333, -77], name: "UM" },
        { latLng: [18.35, -64.933333], name: "VI" },
        { latLng: [41.31666667, 69.25], name: "UZ" },
        { latLng: [-17.73333333, 168.316667], name: "VU" },
        { latLng: [41.9, 12.45], name: "VA" },
        { latLng: [10.48333333, -66.866667], name: "VE" },
        { latLng: [21.03333333, 105.85], name: "VN" },
        { latLng: [-13.95, -171.933333], name: "WF" },
        { latLng: [27.153611, -13.203333], name: "EH" },
        { latLng: [15.35, 44.2], name: "YE" },
        { latLng: [-15.41666667, 28.283333], name: "ZM" },
        { latLng: [-17.81666667, 31.033333], name: "ZW" }
    ];

    var newMarkers = new Array();
   
    for (var i = 0; i < uniqueCountries.length; i++) {
        var key = uniqueCountries[i];
        for (var j = 0; j < markers.length; j++) {
            if (markers[j]["name"] == key) {
                newMarkers.push(markers[j]);
            }
        }
    }

    /* jVector Maps
     * ------------
     * Create a world map with markers
     */
    $('#world-map-markers').vectorMap({
        map: 'world_mill_en',
        normalizeFunction: 'polynomial',
        hoverOpacity: 0.7,
        hoverColor: false,
        backgroundColor: 'transparent',
        regionStyle: {
            initial: {
                fill: 'rgba(210, 214, 222, 1)',
                "fill-opacity": 1,
                stroke: 'none',
                "stroke-width": 0,
                "stroke-opacity": 1
            },
            hover: {
                "fill-opacity": 0.7,
                cursor: 'pointer'
            },
            selected: {
                fill: 'yellow'
            },
            selectedHover: {}
        },
        markerStyle: {
            initial: {
                fill: '#00a65a',
                stroke: '#111'
            }
        },
        markers : newMarkers
    });


    var listX = new Array();

    for (var i = 0; i < uniqueCountries.length; i++) {
        var colour = "#000000".replace(/0/g, function () { return (~~(Math.random() * 16)).toString(16); });
        var xkey = uniqueCountries[i];
        var xyz = new Object();
        xyz.value = datax.$values.filter(function (d) { return d.VideoLocation === xkey; }).length;
        var label = datax.$values.filter(function (d) { return d.VideoLocation === xkey; })[0].VideoLocation;

        if (label) {
            xyz.label = label;
        } else {
            xyz.label = "unknown location";
        }
        
        xyz.color = colour;
        xyz.highlight = colour;
        listX.push(xyz);

    }

    //-------------
    //- PIE CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.
    var pieChartCanvas = $("#pieChart").get(0).getContext("2d");
    var pieChart = new Chart(pieChartCanvas);
    var PieData = listX;
    var pieOptions = {
        //Boolean - Whether we should show a stroke on each segment
        segmentShowStroke: true,
        //String - The colour of each segment stroke
        segmentStrokeColor: "#fff",
        //Number - The width of each segment stroke
        segmentStrokeWidth: 1,
        //Number - The percentage of the chart that we cut out of the middle
        percentageInnerCutout: 50, // This is 0 for Pie charts
        //Number - Amount of animation steps
        animationSteps: 100,
        //String - Animation easing effect
        animationEasing: "easeOutBounce",
        //Boolean - Whether we animate the rotation of the Doughnut
        animateRotate: true,
        //Boolean - Whether we animate scaling the Doughnut from the centre
        animateScale: false,
        //Boolean - whether to make the chart responsive to window resizing
        responsive: true,
        // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
        maintainAspectRatio: false,
        //String - A legend template
        legendTemplate:
            "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>",
        //String - A tooltip template
        tooltipTemplate: "<%=value %> videos from <%=label%> "
    };
    //Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    pieChart.Doughnut(PieData, pieOptions);
    //-----------------
    //- END PIE CHART -
    //-----------------




    /* SPARKLINE CHARTS
     * ----------------
     * Create a inline charts with spark line
     */

    //-----------------
    //- SPARKLINE BAR -
    //-----------------
    $('.sparkbar').each(function() {
        var $this = $(this);
        $this.sparkline('html',
        {
            type: 'bar',
            height: $this.data('height') ? $this.data('height') : '30',
            barColor: $this.data('color')
        });
    });

    //-----------------
    //- SPARKLINE PIE -
    //-----------------
    $('.sparkpie').each(function() {
        var $this = $(this);
        $this.sparkline('html',
        {
            type: 'pie',
            height: $this.data('height') ? $this.data('height') : '90',
            sliceColors: $this.data('color')
        });
    });

    //------------------
    //- SPARKLINE LINE -
    //------------------
    $('.sparkline').each(function() {
        var $this = $(this);
        $this.sparkline('html',
        {
            type: 'line',
            height: $this.data('height') ? $this.data('height') : '90',
            width: '100%',
            lineColor: $this.data('linecolor'),
            fillColor: $this.data('fillcolor'),
            spotColor: $this.data('spotcolor')
        });
    });
});



