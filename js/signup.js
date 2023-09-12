
const signupForm = document.getElementById("signupForm");
const signupPopup = document.getElementById("popup-prompt");

function displaySignupPopup(message) {
    const cleanedMessage = message.replace(/"/g, '');

    signupPopup.innerHTML = cleanedMessage;
    signupPopup.style.marginTop = "30px";
    signupPopup.style.marginBottom = "30px";
    signupPopup.style.color = "#28a745";
    setTimeout(() => {
        signupPopup.innerHTML = '';
        signupPopup.style.marginTop = "0";
        signupPopup.style.marginBottom = "0";
    }, 3000);
}

signupForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    const user = document.getElementById("username").value;
    const pass = document.getElementById("password").value;
    const repass = document.getElementById('re-enterpassword').value;

    if (repass != pass){
        displaySignupPopup("Please double check your password.")
        return
    }

    let fetchPromise = fetch("https://localhost:8080/api/Register", {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        method: "POST",
        body: JSON.stringify({ "userName": user, "password": pass })
    });

    const streamPromise = fetchPromise.then((response) => response.text());

    streamPromise.then((data) => {
        displaySignupPopup(data);
    });

    document.getElementById('password').value = '';
    document.getElementById('username').value = '';
    document.getElementById('re-enterpassword').value = '';
});

