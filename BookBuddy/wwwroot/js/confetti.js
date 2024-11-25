import confetti from 'canvas-confetti';

window.startConfetti = () => {
    confetti({
        particleCount: 100,
        spread: 70,
        origin: { y: 0.6 }
    });
};

window.stopConfetti = () => {
    // canvas-confetti doesn't have a direct "stop" function,
    // but you can clean up or disable effects here if needed.
};