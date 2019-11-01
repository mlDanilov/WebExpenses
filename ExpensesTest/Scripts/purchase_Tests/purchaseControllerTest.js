'use strict'
describe("Real Purchase Controller Test", function () {

    let mockScope = {};
    let backend;
    let model = {};

    //Тестовый набор групп товаров
    let groups = [
        { Id: 0, IdParent: null, Name: "Главная группа" },
        { Id: 1, IdParent: 0, Name: "Мясо" },
        { Id: 2, IdParent: 1, Name: "Птица" },
        { Id: 3, IdParent: 1, Name: "Говядина" },
        { Id: 4, IdParent: 0, Name: "Остальные" }
    ];

    //Тестовый набор магазинов
    let shops = [
        { Id: 1, Name: "Кировский", Address: "На июльской" },
        { Id: 2, Name: "Райт", Address: "На сулимова" },
        { Id: 3, Name: "Аптека", Address: "На июльской" },
        { Id: 4, Name: "Аптека", Address: "На сулимова" },
    ];
    //Тестовый набор товаров
    let items = [
        //Главная группа\Мясо\Птица
        { Id: 0, GId: 2, Name: "Куриные бёдрыши" },
        { Id: 1, GId: 2, Name: "Куриные крылишки" },
        { Id: 3, GId: 2, Name: "Куриные шейки" },
        //Главная группа\Мясо\Говядина
        { Id: 4, GId: 3, Name: "Говяжья вырезка" },
        { Id: 5, GId: 3, Name: "Рёбрышки" },
        { Id: 6, GId: 3, Name: "Смесь для шашлыка" },
        { Id: 7, GId: 3, Name: "Рога и копыта" },
        //Главная группа\Остальное
        { Id: 8, GId: 4, Name: "Мыло" },
        { Id: 9, GId: 4, Name: "Шампунь" },
        { Id: 10, GId: 4, Name: "Туалетная бумага" },
        { Id: 11, GId: 4, Name: "Зубная паста" },
        { Id: 12, GId: 4, Name: "Чистящее средство" }
    ];

    //Тестовый набор покупок
    let purchases = [
        //Декабрь 2018 1 неделя
        { Id: 0, Shop_Id: null, Item_Id: 2, Price: 10, Count: 5, Date = new Date(2018, 0, 1) },
        { Id: 1, Shop_Id: 2, Item_Id: 7, Price: 20, Count: 7, Date = new Date(2018, 0, 1) },
        { Id: 2, Shop_Id: 2, Item_Id: 5, Price: 15, Count: 1, Date = new Date(2018, 0, 2) },
        { Id: 3, Shop_Id: null, Item_Id: 3, Price: 54, Count: 1, Date = new Date(2018, 0, 2) },
        { Id: 4, Shop_Id: 3, Item_Id: 1, Price: 20, Count: 2, Date = new Date(2018, 0, 3) },
        { Id: 5, Shop_Id: 1, Item_Id: 8, Price: 10, Count: 2, Date = new Date(2018, 0, 3) },
        { Id: 6, Shop_Id: null, Item_Id: 9, Price: 8, Count: 6, Date = new Date(2018, 0, 3) },
        { Id: 7, Shop_Id: 2, Item_Id: 5, Price: 61, Count: 2, Date = new Date(2018, 0, 3) },
        { Id: 8, Shop_Id: 1, Item_Id: 6, Price: 48, Count: 3, Date = new Date(2018, 0, 5) },
        { Id: 9, Shop_Id: null, Item_Id: 7, Price: 48, Count: 3, Date = new Date(2018, 0, 5) },

        //Ноябрь 2017 1 неделя
        { Id: 10, Shop_Id: 4, Item_Id: 7, Price: 33, Count: 5, Date = new Date(2017, 10, 1) },
        { Id: 11, Shop_Id: null, Item_Id: 8, Price: 20, Count: 7, Date = new Date(2017, 10, 1) },
        { Id: 12, Shop_Id: 3, Item_Id: 8, Price: 27, Count: 1, Date = new Date(2017, 10, 1) },
        { Id: 13, Shop_Id: null, Item_Id: 9, Price: 73, Count: 1, Date = new Date(2017, 10, 3) },
        { Id: 14, Shop_Id: 2, Item_Id: 2, Price: 23, Count: 2, Date = new Date(2017, 10, 3) },
        { Id: 15, Shop_Id: null, Item_Id: 1, Price: 56, Count: 2, Date = new Date(2017, 10, 3) },
        { Id: 16, Shop_Id: 4, Item_Id: 3, Price: 43, Count: 6, Date = new Date(2017, 10, 4) },
        { Id: 17, Shop_Id: 1, Item_Id: 4, Price: 61, Count: 1, Date = new Date(2017, 10, 4) },
        { Id: 18, Shop_Id: null, Item_Id: 5, Price: 21, Count: 3, Date = new Date(2017, 10, 4) },
        { Id: 19, Shop_Id: 3, Item_Id: 5, Price: 55, Count: 3, Date = new Date(2017, 10, 4) },

        //Ноябрь 2017 2-3 недели
        { Id: 20, Shop_Id: null, Item_Id: 7, Price: 45, Count: 3, Date = new Date(2017, 10, 16) },
        { Id: 21, Shop_Id: 1, Item_Id: 8, Price: 43, Count: 4, Date = new Date(2017, 10, 16) },
        { Id: 22, Shop_Id: 3, Item_Id: 8, Price: 28, Count: 8, Date = new Date(2017, 10, 16) },
        { Id: 23, Shop_Id: null, Item_Id: 9, Price: 12, Count: 9, Date = new Date(2017, 10, 18) },
        { Id: 24, Shop_Id: 2, Item_Id: 2, Price: 76, Count: 4, Date = new Date(2017, 10, 18) },
        { Id: 25, Shop_Id: 2, Item_Id: 1, Price: 20, Count: 5, Date = new Date(2017, 10, 18) },
        { Id: 26, Shop_Id: null, Item_Id: 3, Price: 65, Count: 7, Date = new Date(2017, 10, 19) },
        { Id: 27, Shop_Id: 4, Item_Id: 4, Price: 36, Count: 3, Date = new Date(2017, 10, 19) },
        { Id: 28, Shop_Id: 2, Item_Id: 5, Price: 87, Count: 3, Date = new Date(2017, 10, 19) },
        { Id: 29, Shop_Id: null, Item_Id: 5, Price: 29, Count: 1, Date = new Date(2017, 10, 19) },

        //Октябрь 2019 1 неделя
        { Id: 31, Shop_Id: 1, Item_Id: 8, Price: 85, Count: 12, Date = new Date(2019, 9, 1) },
        { Id: 32, Shop_Id: null, Item_Id: 4, Price: 45, Count: 4, Date = new Date(2019, 9, 1) },
        { Id: 33, Shop_Id: 3, Item_Id: 8, Price: 32, Count: 8, Date = new Date(2019, 9, 1) },
        { Id: 34, Shop_Id: 3, Item_Id: 1, Price: 73, Count: 54, Date = new Date(2019, 9, 2) },
        { Id: 35, Shop_Id: 4, Item_Id: 12, Price: 54, Count: 2, Date = new Date(2019, 9, 4) },
        { Id: 36, Shop_Id: 2, Item_Id: 12, Price: 64, Count: 5, Date = new Date(2019, 9, 5) },
        { Id: 37, Shop_Id: null, Item_Id: 4, Price: 74, Count: 45, Date = new Date(2019, 9, 3) },
        { Id: 38, Shop_Id: 3, Item_Id: 7, Price: 45, Count: 7, Date = new Date(2019, 9, 3) },
        { Id: 39, Shop_Id: null, Item_Id: 5, Price: 18, Count: 9, Date = new Date(2019, 9, 4) },
        { Id: 40, Shop_Id: 1, Item_Id: 8, Price: 46, Count: 12, Date = new Date(2019, 9, 5) }
    ];


    beforeEach(angular.mock.module("purchApp"));

    //Задаем тестовую покупку, в качестве передаваемой
    //в контроллер "purchaseController"
    //проверяем, что все объекты, связанные с редактированем покупки
    //(список магазинов, список групп товаров, товары)
    //заполняются корректно
    beforeEach(function () {
        model = {
            Id: 1,
            Date: new Date(2018, 0, 1),
            Count: 7,
            Price: 20,
            Item: {
                Id: 7,
                GId: 3,
                Name: "Рога и копыта"
            },
            Shop: {
                Id: 2,
                Name: "Пятерочка",
                Address: "Baku Plaza"
            }
        };
    });

    beforeEach(angular.mock.inject(function ($httpBackend) {
            backend = $httpBackend;
        //Мок со списком всех доступных групп товаров
        backend.expect("GET", "/Group/GetGroupList").respond(
            { GroupList: groups }
        );

        //Мок со списком всех доступных магазинов
        backend.expect("GET", "/Shop/GetList").respond(
            { Shops: shops }
        );
        //Мок со списком товаров по выбранной группе
        backend.expect("GET", "/Item/GetItemListByGroupId?groupId_=" + model.Item.GId).respond(
            { ItemList: items.filter(it => it.GId == model.Item.GId) }
        );

    }));


    //Создаем контроллер
    beforeEach(angular.mock.inject(
        function ($controller, $rootScope, $http) {
            mockScope = $rootScope.$new();

            $controller("purchaseController", {
                $scope: mockScope,
                $http: $http
            });

            //mockScope.getGroups(2);
            mockScope.init(model);
            //mockScope.getGroups(2);
            backend.flush();
            
        }
    ));

    it("Ajax тест(ответы получены)", function () {
        backend.verifyNoOutstandingExpectation();
    });


    it("тестируем Groups init(model)", function () {
        console.log();
        console.log("тестируем Groups init(model)");
        expect(mockScope.Id).toEqual(1);
        expect(mockScope.Price).toEqual(20);
        expect(mockScope.Count).toEqual(7);
        expect(mockScope.Groups.Items.length).toEqual(5);
        console.log("mockScope.Id=" + mockScope.Id);
        console.log("mockScope.Price=" + mockScope.Price);
        console.log("mockScope.Count=" + mockScope.Count);
        console.log("mockScope.Groups.Items.length=" + mockScope.Groups.Items.length);
        console.log("mockScope.Groups=");
        console.log(mockScope.Groups.Items[0].Name);
        console.log(mockScope.Groups.Items[1].Name);
        console.log(mockScope.Groups.Items[2].Name);
        console.log(mockScope.Groups.Items[3].Name);
        console.log(mockScope.Groups.Items[4].Name);
    });

    it("тестируем Shops init(model)", function () {
        console.log();
        console.log("тестируем Shops init(model)");
        console.log("mockScope.Shops.Items.length=" + mockScope.Shops.Items.length);
        expect(mockScope.Shops.Items.length).toEqual(4);
        console.log(mockScope.Shops.Items[0].Name);
        console.log(mockScope.Shops.Items[1].Name);
        console.log(mockScope.Shops.Items[2].Name);
        console.log(mockScope.Shops.Items[3].Name);
    });

    it("тестируем Items init(model)", function () {
        console.log();
        console.log("тестируем Items init(model)");
        console.log("mockScope.Items.Items.length=" + mockScope.Items.Items.length);
        expect(mockScope.Items.Items.length).toEqual(4);
        console.log(mockScope.Items.Items[0].Name);
        console.log(mockScope.Items.Items[1].Name);
        console.log(mockScope.Items.Items[2].Name);
        console.log(mockScope.Items.Items[3].Name);
    });

    //it("тестовый 'тест' на реальном контроллере purchaseController", function () {
    //    expect(mockScope.test).toEqual("preved medved*");
    //    //assert.equal(mockScope.test, "preved medved!", "тест не прошёл")
        
    //});

    

});