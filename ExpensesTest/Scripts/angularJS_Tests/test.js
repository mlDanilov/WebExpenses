describe("my first test on karma", function () {


    let preved;
    let medved;

    let a = 0;
    let b = 0;

    beforeEach(function () {
        preved = "preved";
        medved = "medved";
        a = 2;
        b = 6;
    });


    //preved + medved должно быть "preved medved!!!"
    it("Is corrected preved + medved?",  function () {
        let res = preved + " " + medved + "!!!";
        expect(res).toEqual("preved medved!!!");
        //expect(res).toEqual("preved medved???");
    });


    //preved + medved должно быть "preved medved!!!"
    it("a + b ?", function () {
        expect(a + b).toEqual(8);
    });

});