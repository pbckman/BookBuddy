
window.startContinuousScroll = function () {
    const carouselContent = document.querySelector('.carousel-content');
    if (!carouselContent) {
        console.error("Carousel content not found.");
        return;
    }

    const cardElements = document.querySelectorAll('.carousel-card');
    if (cardElements.length === 0) {
        console.error("Carousel cards not found.");
        return;
    }

    let originalContent = carouselContent.innerHTML;
    const cardWidth = cardElements[0].offsetWidth + 20;
    let scrollPosition = 0;
    const scrollSpeed = 1;

    function startScrolling() {
        setInterval(() => {
            scrollPosition += scrollSpeed;


            if (scrollPosition >= carouselContent.scrollWidth / 6) {

                carouselContent.innerHTML += originalContent;
            }

            carouselContent.style.transform = `translateX(-${scrollPosition}px)`;
        }, 20); 
    }

    startScrolling();
};
