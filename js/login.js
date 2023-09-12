const loginForm = document.getElementById("loginForm");
const popupPrompt = document.getElementById("popup-prompt");

function displayPopup(message, isError = false) {
    const popupClass = isError ? "error" : "success";
    popupPrompt.innerHTML = `<div class="${popupClass}">${message}</div>`;

    if (message) {
        popupPrompt.style.marginTop = "30px";
        popupPrompt.style.marginBottom = "30px";
        popupPrompt.style.color = "#007bff";
    } else {
        popupPrompt.style.marginTop = "0";
        popupPrompt.style.marginBottom = "0";
    }

    setTimeout(() => {
        popupPrompt.innerHTML = ''; 
        popupPrompt.style.marginTop = "0";
        popupPrompt.style.marginBottom = "0";
    }, 3000);
}

loginForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    let user = document.getElementById("usernamelog").value;
    let pass = document.getElementById("passwordlog").value;

    if (user === "" || pass === "") {
        return;
    }

    await new Promise(resolve => setTimeout(resolve, 100));

    const fetchPromise = fetch("https://localhost:8080/api/CloseAuction/1", {
        headers: {
            'Authorization': 'Basic ' + btoa(user + ":" + pass),
        },
    });

    fetchPromise.then((response) => {
        if (!response.ok) {
            catchError(response.status);
        } else {
            displayPopup("User successfully logged in", false);
            localStorage.setItem("isLoggedIn", "true");
        }
    }).catch(catchError);

    function catchError(error) {
        displayPopup("Username or password is incorrect", true);
    }

    document.getElementById('passwordlog').value = '';
    document.getElementById('usernamelog').value = '';
});