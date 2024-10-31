var userLang;

// Försök att få språket från URL-parametern först
var urlParams = new URLSearchParams(window.location.search);
userLang = urlParams.get('language');

if (!userLang) {
    // Kontrollera om path innehåller '/sv/' eller '/en/' för att sätta språk
    if (window.location.pathname.includes('/sv/')) {
        userLang = 'sv';
    } else if (window.location.pathname.includes('/en/')) {
        userLang = 'en';
    }
}

var langAttribute;
var scriptSrc;
var dataId = "8c1cad13-6fe3-4f93-8ef4-1b81e0cca58d"; // Ditt data-id

// Bestäm rätt språk för samtyckespop-upen
if (userLang === 'sv') {
    langAttribute = "sv-SE";
    scriptSrc = "https://cdn.cookietractor.com/cookietractor.js";
} else {
    langAttribute = "en-GB";
    scriptSrc = "https://cdn.cookietractor.com/cookietractor.js";
}

// Ladda Cookietractor-skriptet för samtyckespop-upen
var consentScript = document.createElement('script');
consentScript.src = scriptSrc;
consentScript.setAttribute('data-lang', langAttribute);
consentScript.setAttribute('data-id', dataId);
document.head.appendChild(consentScript);

// Bestäm om det är en deklarationssida och ladda rätt skript för den
if (document.getElementById("CookieDeclaration")) {
    // Byt ut scriptSrc till deklarations-skriptet
    scriptSrc = "https://cdn.cookietractor.com/cookietractor-declaration.js";

    var declarationScript = document.createElement('script');
    declarationScript.src = scriptSrc;
    declarationScript.setAttribute('data-lang', langAttribute);
    declarationScript.setAttribute('data-id', dataId);
    declarationScript.defer = true;
    document.head.appendChild(declarationScript);
}



window.openCookieSettings = function () {

    if (window.cookieTractor) {
        window.cookieTractor.openConsentSettings();
    }
}