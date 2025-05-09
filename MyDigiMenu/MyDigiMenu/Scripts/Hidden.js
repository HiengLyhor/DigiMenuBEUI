window.onload = function () {
    // Hide the link with specific text
    $("a").filter(function () {
        return $(this).text() === "Web hosting by Somee.com";  // Match by text
    }).hide();  // Hide the matching link

    // Hide the <div> containing specific text
    $("div").filter(function () {
        return $(this).text().includes("Web hosting by Somee.com") || $(this).text().includes("Hosted Windows Virtual Server");  // Match by text
    }).hide();  // Hide the entire <div>

    // Hide the <div> with specific styles (opacity, position, and bottom)
    $("div").filter(function () {
        return $(this).css("opacity") === "0.9" &&
            $(this).css("position") === "fixed" &&
            $(this).css("bottom") === "0px";  // Match by opacity, position, and bottom
    }).hide();  // Hide the matching div
};  

$(document).ready(function () {
    $("a").filter(function () {
        return $(this).text() === "Web hosting by Somee.com";  // Match by text
    }).hide();  // Hide the matching link

    $("div").filter(function () {
        return $(this).text().includes("Web hosting by Somee.com") || $(this).text().includes("Hosted Windows Virtual Server");  // Match by text
    }).hide();  // Hide the entire <div>

    $("div").filter(function () {
        return $(this).css("opacity") === "0.9" &&
            $(this).css("position") === "fixed" &&
            $(this).css("bottom") === "0px";  // Match by opacity, position, and bottom
    }).hide();  // Hide the matching div

});