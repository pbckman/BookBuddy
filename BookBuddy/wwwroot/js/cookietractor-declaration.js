document.addEventListener("DOMContentLoaded", function () {
    console.log("Loading Cookie Declaration script on cookie info page...");

    if (document.getElementById("CookieDeclaration") && !document.getElementById('cookie-declaration-script')) {
        var userLang = new URLSearchParams(window.location.search).get('language') ||
            (window.location.pathname.includes('/sv/') ? 'sv' : 'en');
        var langAttribute = userLang === 'sv' ? "sv-SE" : "en-GB";
        var dataId = "e470af12-ff81-4cd2-b3df-79f942173b8a";

        if (!document.getElementById('cookie-consent-script')) {
            var consentScript = document.createElement('script');
            consentScript.src = "https://cdn.cookietractor.com/cookietractor.js";
            consentScript.id = 'cookie-consent-script';
            consentScript.setAttribute('data-lang', langAttribute);
            consentScript.setAttribute('data-id', dataId);
            consentScript.defer = true;

            consentScript.onload = function () {
                console.log("CookieTractor script loaded successfully!");

                if (window.cookieTractor) {
                    var declarationScript = document.createElement('script');
                    declarationScript.src = "https://cdn.cookietractor.com/cookietractor-declaration.js";
                    declarationScript.id = 'cookie-declaration-script';
                    declarationScript.setAttribute('data-lang', langAttribute);
                    declarationScript.setAttribute('data-id', dataId);
                    declarationScript.defer = true;

                    declarationScript.onload = function () {
                        console.log("Cookie Declaration script loaded successfully!");

                        if (window.cookieTractor) {
                            console.log("cookieTractor is available for cookie declaration.");
                        } else {
                            console.error("cookieTractor is not available for cookie declaration.");
                        }
                    };

                    document.head.appendChild(declarationScript);
                } else {
                    console.error("cookieTractor is not defined after loading consent script.");
                }
            };

            document.head.appendChild(consentScript);
        } else {
            console.log("Cookie Consent script is already loaded.");
        }
    } else {
        console.log("Cookie Declaration script is already loaded or not required.");
    }
});