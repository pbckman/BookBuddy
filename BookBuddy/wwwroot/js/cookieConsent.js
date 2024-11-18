function isConsentGiven() {
    const consentCookieExists = document.cookie.split('; ').some(cookie => cookie.startsWith('_cc_cookieConsent='));
    const consentGivenFlag = localStorage.getItem('consentGiven');
    return consentCookieExists || consentGivenFlag === 'true';
}

function setConsentFlag() {
    localStorage.setItem('consentGiven', 'true');
    document.cookie = "_cc_cookieConsent=true; path=/;";
}

function checkAndShowConsentDialog() {

    if (!isConsentGiven()) {
        console.log("Consent not given, showing dialog...");
        window.cookieTractor.openConsentSettings();
        setConsentFlag();
    } else {
        console.log("Consent already given, no need to show the dialog again.");
        var cookiePopup = document.getElementById('cookie-consent-popup');
        if (cookiePopup) {
            cookiePopup.style.display = 'none';
        }
    }
}

function checkConsentStatus() {
    var consentGiven = localStorage.getItem('cookieConsentGiven');

    if (consentGiven === 'true') {
        console.log("Consent already given, no need to show the dialog.");
    } else {
        console.log("Consent not given, showing dialog.");
        window.cookieTractor.openConsentSettings();
    }
}

function storeConsent(consentStatus) {
    localStorage.setItem('cookieConsentGiven', consentStatus ? 'true' : 'false');
}
