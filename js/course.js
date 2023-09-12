function setupModal(buttonId, modalId) {
    var modal = document.getElementById(modalId);
    var btn = document.getElementById(buttonId);
    var span = modal.querySelector(".close");
    var expandButtons = modal.querySelectorAll(".expand");
  
    btn.onclick = function () {
      modal.style.display = "block";
    };
  
    span.onclick = function () {
      modal.style.display = "none";
    };
  
    modal.onclick = function (event) {
      if (event.target === modal) {
        modal.style.display = "none";
      }
    };
  
    expandButtons.forEach(function (button) {
      button.onclick = function () {
        var courseId = button.getAttribute("data-course");
        var courseDescription = document.getElementById(courseId);
  
        if (courseDescription.style.display === "none" || courseDescription.style.display === "") {
          courseDescription.style.display = "block";
          button.textContent = "Collapse";
        } else {
          courseDescription.style.display = "none";
          button.textContent = "Expand";
        }
      };
    });
}

setupModal("commerceBtn", "commerceModal");

setupModal("compsciBtn", "compsciModal");