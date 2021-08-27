window.getBrowserLanguage = function () {
    var language = (navigator.languages && navigator.languages.length) ? navigator.languages[0] :
        navigator.userLanguage || navigator.language || navigator.browserLanguage || 'en';

    if (language.includes("-")) {
        return language.split("-").pop();
    }
    else {
        return language;
    }
}