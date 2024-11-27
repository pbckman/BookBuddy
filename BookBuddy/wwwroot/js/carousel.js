

window.startCarouselAnimation = function () {
    console.log('Starting carousel animation...');
    const carouselContent = document.querySelector('#bookCarousel');


    if (!carouselContent) {
        console.error('Element with id #bookCarousel not found');
        return;
    }


    const cards = Array.from(carouselContent.children);
    cards.forEach(card => {
        const clone = card.cloneNode(true);
        carouselContent.appendChild(clone);
    });

    let startPosition = 0;
    const totalWidth = carouselContent.scrollWidth / 2;

    function moveCarousel() {

        startPosition -= 1;


        carouselContent.style.transform = `translateX(${startPosition}px)`;

        if (Math.abs(startPosition) >= totalWidth) {
            startPosition = 0;
        }

        requestAnimationFrame(moveCarousel);
    }

    moveCarousel();
};
