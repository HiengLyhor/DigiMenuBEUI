window.onload = function () {
    $("a").filter(function () {
        return $(this).text() === "Web hosting by Somee.com";
    }).hide();

    $("div").filter(function () {
        return $(this).text().includes("Web hosting by Somee.com") || $(this).text().includes("Hosted Windows Virtual Server");
    }).hide();

    $("div").filter(function () {
        return $(this).css("opacity") === "0.9" &&
            $(this).css("position") === "fixed" &&
            $(this).css("bottom") === "0px";
    }).hide();
};  

$(document).ready(function () {
    $("a").filter(function () {
        return $(this).text() === "Web hosting by Somee.com";
    }).hide();

    $("div").filter(function () {
        return $(this).text().includes("Web hosting by Somee.com") || $(this).text().includes("Hosted Windows Virtual Server");
    }).hide();

    $("div").filter(function () {
        return $(this).css("opacity") === "0.9" &&
            $(this).css("position") === "fixed" &&
            $(this).css("bottom") === "0px";
    }).hide();

});