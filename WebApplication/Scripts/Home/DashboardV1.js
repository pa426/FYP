/*
 * Author: Abdullah A Almsaeed
 * Date: 4 Jan 2014
 * Description:
 *      This is a demo file used only for the main dashboard
 **/

$(function () {

    "use strict";

    //Make the dashboard widgets sortable Using jquery UI
    $(".connectedSortable").sortable({
        placeholder: "sort-highlight",
        connectWith: ".connectedSortable",
        handle: ".box-header, .nav-tabs",
        forcePlaceholderSize: true,
        zIndex: 999999
    });
    $(".connectedSortable .box-header, .connectedSortable .nav-tabs-custom").css("cursor", "move");

    //jQuery UI sortable for the todo list
    $(".todo-list").sortable({
        placeholder: "sort-highlight",
        handle: ".handle",
        forcePlaceholderSize: true,
        zIndex: 999999
    });

    /* jQueryKnob */
    $(".knob").knob();


    //SLIMSCROLL FOR CHAT WIDGET
    $('#chat-box').slimScroll({
        height: '250px'
    });

    /* Morris.js Charts */
    // video chart
    var xmodel = modelVideo.slice();
    xmodel.pop();
    var area1 = new Morris.Line({
        element: 'videoEmotion-chart',
        resize: true,
        data: xmodel,
        xkey: 'y',
        ykeys: ['item1', 'item2', 'item3', 'item4', 'item5', 'item6', 'item7', 'item8'],
        labels: ['Anger', 'Contempt', 'Disgust', 'Fear', 'Happines', 'Neutral', 'Sadness', 'Surprise'],
        lineColors: ['#FF0000', '#c30379', '#7622b0', '#fee200', '#30ba24', '#d2c7f4', '#2574bc', '#fee280'],
        parseTime: false,
        hideHover: 'auto'
    }).on('click',
        function (i, row) {
            var newTime = (row['y'] / 2);
            player.seekTo(newTime);
            player.pauseVideo();
            player.playVideo();
            player.pauseVideo();
        });


    //Donut Chart
    var donut1 = new Morris.Donut({
        element: 'videoEmotion-donut',
        resize: true,
        colors: ['#FF0000', '#c30379', '#7622b0', '#fee200', '#30ba24', '#d2c7f4', '#2574bc', '#fee280'],
        data: [
            { label: "Anger", value: modelVideo[modelVideo.length - 1].item1 },
            { label: "Contempt", value: modelVideo[modelVideo.length - 1].item2 },
            { label: "Disgust", value: modelVideo[modelVideo.length - 1].item3 },
            { label: "Fear", value: modelVideo[modelVideo.length - 1].item4 },
            { label: "Happines", value: modelVideo[modelVideo.length - 1].item5 },
            { label: "Neutral", value: modelVideo[modelVideo.length - 1].item6 },
            { label: "Sadness", value: modelVideo[modelVideo.length - 1].item7 },
            { label: "Surprise", value: modelVideo[modelVideo.length - 1].item8 }
        ],
        hideHover: 'auto'
    }).js;


    /* Morris.js Charts */
    // text chart
    var ymodel = modelText.slice();
    ymodel.pop();
    var area2 = new Morris.Line({
        element: 'textEmotion-chart',
        resize: true,
        data: ymodel,
        xkey: 'y',
        ykeys: ['item1', 'item2', 'item3', 'item4', 'item5'],
        labels: ['Anger', 'Disgust', 'Fear', 'Joy', 'Sadness'],
        lineColors: ['#ff3400', '#7622b0', '#ffe62c', '#169c02', '#2574bc'],
        hideHover: 'auto',
        parseTime: false
    }).on('click',
        function (i, row) {
            var tab = ("#tby_" + i);
            $(tab).click();
        });

    //Donut Chart
    var donut2 = new Morris.Donut({
        element: 'textEmotion-donut',
        resize: true,
        colors: ['#ff3400', '#7622b0', '#ffe62c', '#169c02', '#2574bc'],
        data: [
            { label: "Anger", value: modelText[modelText.length - 1].item1 },
            { label: "Disgust", value: modelText[modelText.length - 1].item2 },
            { label: "Fear", value: modelText[modelText.length - 1].item3 },
            { label: "Joy", value: modelText[modelText.length - 1].item4 },
            { label: "Sadness", value: modelText[modelText.length - 1].item5 }
        ],
        hideHover: 'auto'
    }).js;

    /* Morris.js Charts */
    // text chart
    var zmodel = modelSound.slice();
    zmodel.pop();
    var area3 = new Morris.Line({
        element: 'soundEmotion-chart',
        resize: true,
        data: zmodel,
        xkey: 'y',
        ykeys: ['item1', 'item2', 'item3'],
        labels: ['Temper', 'Valence', 'Arousal'],
        lineColors: ['#ff3400', '#7622b0', '#ffe62c'],
        hideHover: 'auto',
        parseTime: false
    }).on('click',
        function (i, row) {
            var tab = ("#tbz_" + i);
            $(tab).click();
        });


    //Donut Chart
    var donut3 = new Morris.Donut({
        element: 'soundEmotion-donut',
        resize: true,
        colors: ['#ff3400', '#7622b0', '#ffe62c'],
        data: [
            { label: "Temper", value: modelSound[modelSound.length - 1].item1 },
            { label: "Valence", value: modelSound[modelSound.length - 1].item2 },
            { label: "Arousal", value: modelSound[modelSound.length - 1].item3 }
        ],
        hideHover: 'auto'
    }).js;

    //Donut Chart
    var donut4 = new Morris.Donut({
        element: 'videoEmotion-donutS',
        resize: true,
        colors: ['#FF0000', '#c30379', '#7622b0', '#fee200', '#30ba24', '#d2c7f4', '#2574bc', '#fee280'],
        data: [
            { label: "Anger", value: modelVideo[modelVideo.length - 1].item1 },
            { label: "Contempt", value: modelVideo[modelVideo.length - 1].item2 },
            { label: "Disgust", value: modelVideo[modelVideo.length - 1].item3 },
            { label: "Fear", value: modelVideo[modelVideo.length - 1].item4 },
            { label: "Happines", value: modelVideo[modelVideo.length - 1].item5 },
            { label: "Neutral", value: modelVideo[modelVideo.length - 1].item6 },
            { label: "Sadness", value: modelVideo[modelVideo.length - 1].item7 },
            { label: "Surprise", value: modelVideo[modelVideo.length - 1].item8 }
        ],
        hideHover: 'auto'
    }).js;

    //Donut Chart
    var donut5 = new Morris.Donut({
        element: 'textEmotion-donutS',
        resize: true,
        colors: ['#ff3400', '#7622b0', '#ffe62c', '#169c02', '#2574bc'],
        data: [
            { label: "Anger", value: modelText[modelText.length - 1].item1 },
            { label: "Disgust", value: modelText[modelText.length - 1].item2 },
            { label: "Fear", value: modelText[modelText.length - 1].item3 },
            { label: "Joy", value: modelText[modelText.length - 1].item4 },
            { label: "Sadness", value: modelText[modelText.length - 1].item5 }
        ],
        hideHover: 'auto'
    }).js;

    //Donut Chart
    var donut6 = new Morris.Donut({
        element: 'soundEmotion-donutS',
        resize: true,
        colors: ['#ff3400', '#7622b0', '#ffe62c'],
        data: [
            { label: "Temper", value: modelSound[modelSound.length - 1].item1 },
            { label: "Valence", value: modelSound[modelSound.length - 1].item2 },
            { label: "Arousal", value: modelSound[modelSound.length - 1].item3 }
        ],
        hideHover: 'auto'
    }).js;



});

///Youtube video frame
var tag = document.createElement('script');
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
var player;

function onYouTubeIframeAPIReady() {
    player = new YT.Player('youtubeFrame',
    {
        height: '100%',
        width: '100%',
        playerVars: { rel: 0 },
        videoId: modelId
    });
}

