//function isConsentGiven() {
//    const consentCookieExists = document.cookie.split('; ').some(cookie => cookie.startsWith('_cc_cookieConsent='));
//    const consentGivenFlag = localStorage.getItem('consentGiven');
//    return consentCookieExists || consentGivenFlag === 'true';
//}

//function setConsentFlag() {
//    localStorage.setItem('consentGiven', 'true');
//    document.cookie = "_cc_cookieConsent=true; path=/;";
//}

//function checkAndShowConsentDialog() {

//    if (!isConsentGiven()) {
//        console.log("Consent not given, showing dialog...");
//        window.cookieTractor.openConsentSettings();
//        setConsentFlag();
//    } else {
//        console.log("Consent already given, no need to show the dialog again.");
//        var cookiePopup = document.getElementById('cookie-consent-popup');
//        if (cookiePopup) {
//            cookiePopup.style.display = 'none';
//        }
//    }
//}

//function checkConsentStatus() {
//    var consentGiven = localStorage.getItem('cookieConsentGiven');

//    if (consentGiven === 'true') {
//        console.log("Consent already given, no need to show the dialog.");
//    } else {
//        console.log("Consent not given, showing dialog.");
//        window.cookieTractor.openConsentSettings();
//    }
//}

//function storeConsent(consentStatus) {
//    localStorage.setItem('cookieConsentGiven', consentStatus ? 'true' : 'false');
//}

function getCookie(name) {
    let cookieArr = document.cookie.split(";");
    for (let i = 0; i < cookieArr.length; i++) {
        let cookiePair = cookieArr[i].split("=");
        if (name == cookiePair[0].trim()) {
            return decodeURIComponent(cookiePair[1]);
        }
    }
    return null;
}

function checkAndShowConsentDialog() {
    let consentCookie = getCookie("_cc_cookieConsent");

    if (consentCookie) {
        try {
            let consentData = JSON.parse(consentCookie);
            if (consentData.consentGiven && consentData.consentGiven === true) {
                console.log("Consent already given, no need to show dialog again.");

                const cookieBanner = document.getElementById("cookieConsentBanner");
                if (cookieBanner) {
                    cookieBanner.style.display = "none";
                }
                return;
            }
        } catch (e) {
            console.error("Error parsing cookie consent data", e);
        }
    }

    console.log("Consent not given, showing dialog...");

    const cookieBanner = document.getElementById("cookieConsentBanner");
    if (cookieBanner) {
        cookieBanner.style.display = "block";
    }
}


function setConsentFlag() {
    localStorage.setItem('consentGiven', 'true');
    const consentData = {
        consentGiven: true,
        timestamp: new Date().toISOString()
    };
    document.cookie = "_cc_cookieConsent=" + encodeURIComponent(JSON.stringify(consentData)) + "; path=/; max-age=" + (365 * 24 * 60 * 60);
    document.getElementById("cookieConsentBanner").style.display = "none";
}

function storeConsent(consentStatus) {
    localStorage.setItem('cookieConsentGiven', consentStatus ? 'true' : 'false');
}

