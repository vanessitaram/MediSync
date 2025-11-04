document.addEventListener("DOMContentLoaded", () => {
    const linkToRegister = document.querySelector("a[href*='Registro']");
    const linkToLogin = document.querySelector("a[href*='Login']");

    if (linkToRegister) {
        linkToRegister.addEventListener("click", (e) => {
            e.preventDefault();
            document.body.classList.add("transitioning");
            document.querySelector(".login-card").classList.add("slide-out");
            setTimeout(() => window.location.href = linkToRegister.href, 500);
        });
    }

    if (linkToLogin) {
        linkToLogin.addEventListener("click", (e) => {
            e.preventDefault();
            document.body.classList.add("transitioning");
            document.querySelector(".login-card").classList.add("slide-out");
            setTimeout(() => window.location.href = linkToLogin.href, 500);
        });
    }
});
