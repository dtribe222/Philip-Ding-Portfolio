var expandButtons = document.querySelectorAll(".expand");

expandButtons.forEach(function (button) {
  button.onclick = function () {
    var projectID = button.getAttribute("data-course");
    var projectDescription = document.getElementById(projectID);

    if (projectDescription.style.display === "none" || projectDescription.style.display === "") {
      projectDescription.style.display = "block";
      button.textContent = "Collapse";
    } else {
      projectDescription.style.display = "none";
      button.textContent = "Expand";
    }
  };
});