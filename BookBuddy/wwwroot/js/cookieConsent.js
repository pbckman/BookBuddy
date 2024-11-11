
//document.addEventListener("DOMContentLoaded", function () {
//    const isLocal = window.location.protocol === 'http:';

//    CookieTractor.init({
//        cookieName: '_cc_cookieConsent',
//        cookieValue: 'true',
//        cookieOptions: {
//            secure: !isLocal, 
//            sameSite: 'None'
//        }
//    });
//});



console.log("cookieConsent.js is loaded and running!");

// Funktion för att avgöra användarens språk
function getUserLanguage() {
    var userLang = new URLSearchParams(window.location.search).get('language') ||
        (window.location.pathname.includes('/sv/') ? 'sv' : 'en');
    return userLang === 'sv' ? "sv-SE" : "en-GB";
}

// Kontrollera om CookieTractor-skriptet redan är inladdat
if (!document.getElementById('cookie-consent-script')) {
    console.log("Loading CookieTractor script...");

    var consentScript = document.createElement('script');
    consentScript.src = "https://cdn.cookietractor.com/cookietractor.js";
    consentScript.id = 'cookie-consent-script'; // Unikt id för detta script
    consentScript.async = true;

    // Sätt rätt språk med 'data-lang'
    var langAttribute = getUserLanguage();
    consentScript.setAttribute('data-lang', langAttribute);

    // Sätt 'data-id' som är unikt för din webbplats
    var dataId = "8c1cad13-6fe3-4f93-8ef4-1b81e0cca58d";
    consentScript.setAttribute('data-id', dataId);

    // Ladda skriptet och kontrollera att cookieTractor är tillgänglig
    consentScript.onload = function () {
        console.log("CookieTractor script loaded successfully!");

        if (window.cookieTractor) {
            console.log("cookieTractor is available!");

            // Öppna consent-settings om inget samtycke är givet
            if (!isConsentGiven()) {
                window.cookieTractor.openConsentSettings();
                setConsentFlag(); // Sätt flaggan för att visa att consent popup har visats
            }
        } else {
            console.log("cookieTractor is NOT available.");
        }
    };

    document.head.appendChild(consentScript);
} else {
    console.log("CookieTractor script already loaded, no need to load again.");
}

// Funktion för att kontrollera om samtycke redan är givet
function isConsentGiven() {
    const consentCookieExists = document.cookie.split('; ').some(cookie => cookie.startsWith('_cc_cookieConsent='));
    const consentGivenFlag = localStorage.getItem('consentGiven');
    return consentCookieExists || consentGivenFlag === 'true';
}

// Funktion för att sätta samtyckesflaggan i localStorage
function setConsentFlag() {
    localStorage.setItem('consentGiven', 'true');
}


window.openCookieSettings = function () {

    if (window.cookieTractor) {
        window.cookieTractor.openConsentSettings();
    }
}
