'use strict';
purchApp.factory('Years', function ($http) {
    let _currentYear = "-";
    let _years = [];

    return {

        //setCurrentYear: function (year) {
        //    _currentYear = year;
        //},
        //getCurrentYear: function () {
        //    return _currentYear;
        //},
        currentYear: _currentYear,
        getYears: function () {
            return _years;
        },
        setYears: function (years_){
            _years = years_;
        }
        
    }
});
