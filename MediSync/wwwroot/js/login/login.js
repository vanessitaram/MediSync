document.addEventListener("mousemove", (e) => {
    const bg = document.querySelector('.login-left');
    const x = e.clientX / window.innerWidth;
    const y = e.clientY / window.innerHeight;
    bg.style.background = `linear-gradient(${120 + x * 20}deg, #2A8DDE, #6C3ECF, #8B2BCB)`;
});
