using System;
using System.Collections.Generic;
using Xunit;

namespace Lab3.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Library library = new Library();
            IEnumerable<Library.BookSearchResult> foundBooksResult;
            // Добавим книги для теста
            Assert.True(library.Add(new Book("Джейн Эйр", "Шарлотта Бронте", "Роман", new DateTime(1931, 1, 25), "9780102439701", "История сиротки Джейн, в раннем возрасте оставшейся без родителей на попечении своей злобной тетушки миссис Рид, позже отданной на воспитание в убогий пансионат, а в более зрелом возрасте зарабатывавшей себе на жизнь гувернанткой и учительницей.")));
            Assert.True(library.Add(new Book("Шерли", "Шарлотта Бронте", "Роман", new DateTime(1933, 2, 22), "9780102439703", "Фабрикант Роберт Мур собирается жениться на богатой наследнице Шерли, хотя его сердце принадлежит бесприданнице Кэролайн, а сама Шерли влюблена в брата Роберта, нищего учителя.")));
            Assert.True(library.Add(new Book("Лунный камень", "Уилки Коллинз", "Детектив, Роман", new DateTime(1947, 6, 15), "9780007449910", "Молодая девушка, Рэйчел (живет в родовом поместье в английской глуши с матерью и слугами), согласно завещанию своего дяди, воевавшего в Индии, получает в день своего совершеннолетия крупный алмаз (Лунный камень) необычайной красоты.Этот алмаз индийская святыня, которую присвоили британские офицеры, и три индусских жреца идут по его следу, не ценя человеческую жизнь и будучи готовыми на все, чтобы вернуть алмаз в храм.")));
            Assert.True(library.Add(new Book("Женщина в белом", "Уилки Коллинз", "Детектив, Роман", new DateTime(1957, 10, 22), "9780007449912", "Молодой художник Уолтер направляется в имение богатого пожилого эсквайра, где получил интересную и денежную работу. По дороге он встречает странную женщину, одетую во всё белое. В имение мистера, Уолтер работает с гравюрами хозяина и даёт уроки рисования его племяннице Лоре. Уолтер удивлен, насколько Лора похожа на странную девушку в белом, но Мэриан отрицает наличие у них других родственников. Уолтер влюбленный в Лору, начинает расследование.")));
            Assert.True(library.Add(new Book("Мегрэ и человек на скамейке", "Жорж Сименон", "Детектив, Драма", new DateTime(1963, 9, 2), "9780003349513", "В глухом парижском переулке рядом с бульваром Сен-Мартен найден убитый мужчина по имени Луи Туре, у убитого красный галстук и жёлтые ботинки, которых, по утверждению его жены, он никогда не носил. В кошельке — неизвестно откуда взявшаяся приличная сумма денег и фотография девочки. Очевидно, что покойный вёл вторую жизнь, которая была скрыта от его семьи.")));
            Assert.True(library.Add(new Book("Мегрэ и старая дама", "Жорж Сименон", "Детектив, Драма", new DateTime(1949, 7, 11), "9780003349511", "Для расследования убийства служанки мадам Бессон - комиссар Мегрэ едет в курортный городок в Нормандии. Там он понимает, что отношения между членами семьи Бессон сложны и запутаны, а к гибели служанки Розы и ее брата Анри, детей простого рыбака, привели жестокость и лживость окружающих.")));
            Assert.True(library.Add(new Book("Мегрэ колеблется", "Жорж Сименон", "Детектив, Драма", new DateTime(1968, 2, 10), "9780003349517", "Комиссар получает письмо, в котором говорится о готовящемся убийстве.Письмо написано на почтовой бумаге редкого качества, и сразу удается узнать, что такой бумагой пользуется известный парижский адвокат Эмиль Парандон. Мегрэ отправляется с визитом в семью адвоката.")));
            Assert.True(library.Add(new Book("Смерть Сесили", "Жорж Сименон", "Детектив, Драма", new DateTime(1969, 1, 13), "9780003349519", "Сесиль Маршан неожиданно обнаруживает, что квартиру, где она живёт вместе с больной тёткой, посещает кто-то чужой. Полицейские, которым было поручено вести наблюдение за этой квартирой, ничего подозрительного не заметили. Однако вскоре Сесиль и её родственницу находят убитыми.")));
            Assert.Equal(8, library.Count());
            // Тестируем поиск по названию
            Predicate<Book> titlePredicate = book => book.Title.Contains("Мегрэ");
            foundBooksResult = library.Search(titlePredicate);
            Assert.Equal(3, foundBooksResult.Count());
            Assert.Equal("Мегрэ и человек на скамейке", foundBooksResult.ElementAt(0).Title);
            Assert.Equal("Мегрэ и старая дама", foundBooksResult.ElementAt(1).Title);
            Assert.Equal("Мегрэ колеблется", foundBooksResult.ElementAt(2).Title);
            // Тестируем поиск по автору
            Predicate<Book> authorPredicate = book => book.Author.Contains("Уилки Коллинз");
            foundBooksResult = library.Search(authorPredicate);
            Assert.Equal(2, foundBooksResult.Count());
            Assert.Equal("Лунный камень", foundBooksResult.ElementAt(0).Title);
            Assert.Equal("Женщина в белом", foundBooksResult.ElementAt(1).Title);
            // Тестируем поиск по ключевым
            string str = "Детектив Драма убийства";
            string[] searchKeyWords = str.Split();
            foundBooksResult = library.SearchKeywords(searchKeyWords);
            Assert.Equal(6, foundBooksResult.Count());
            Assert.Equal("Мегрэ и старая дама", foundBooksResult.ElementAt(0).Title);//3 совпадения
            Assert.Equal("Мегрэ и человек на скамейке", foundBooksResult.ElementAt(1).Title);//2 совпадения
            Assert.Equal("Мегрэ колеблется", foundBooksResult.ElementAt(2).Title);//2 совпадения
            Assert.Equal("Смерть Сесили", foundBooksResult.ElementAt(3).Title);//2 совпадения
            Assert.Equal("Лунный камень", foundBooksResult.ElementAt(4).Title);//1 совпадение
            Assert.Equal("Женщина в белом", foundBooksResult.ElementAt(5).Title);//1 совпадение
            // Тестируем сохранение и загрузку
            Assert.True(library.SaveToXML());
            Assert.True(library.SaveToJSON());
            Assert.True(library.SaveToSQLite());
            //XML
            library.Clear();
            Assert.Equal(0, library.Count());
            Assert.True(library.LoadFromXML());
            Assert.Equal(8, library.Count());
            //JSON
            library.Clear();
            Assert.True(library.LoadFromJSON());
            Assert.Equal(8, library.Count());
            //SQLite
            library.Clear();
            Assert.True(library.LoadFromSQLite());
            Assert.Equal(8, library.Count());

        }
    }
}
