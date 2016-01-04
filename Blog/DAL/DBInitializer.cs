using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Blog.Models;

namespace Blog.DAL
{
    public class DBInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Тестовые пользователи.
            #region
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Пользователи.
            ApplicationUser admin = new ApplicationUser()
            {
                Id = "Admin",
                Email = "admin@email.com",
                UserName = "admin@email.com",
                FullName = "Администратор"
            };
            userManager.Create(admin, "pa$$W0rd");

            ApplicationUser editor = new ApplicationUser()
            {
                Id = "Editor",
                Email = "editor@email.com",
                UserName = "editor@email.com",
                FullName = "Редактор"
            };
            userManager.Create(editor, "pa$$W0rd");

            ApplicationUser user = new ApplicationUser() {
                Id = "User",
                Email = "user@email.com",
                UserName = "user@email.com",
                FullName = "Пользователь"
            };
            userManager.Create(user, "pa$$W0rd");

            // Роли.
            IdentityRole adminRole = new IdentityRole("admin");
            roleManager.Create(adminRole);

            IdentityRole editorRole = new IdentityRole("editor");
            roleManager.Create(editorRole);

            IdentityRole userRole = new IdentityRole("user");
            roleManager.Create(userRole);

            // Пользователь и его роль.
            userManager.AddToRoles("Admin", "user", "editor", "admin");
            userManager.AddToRoles("Editor", "user", "editor");
            userManager.AddToRole("User", "user");
            #endregion

            // Тестовые данные.
            #region
            Posts post1 = new Posts() {
                Date = DateTime.Parse("01.01.2015 07:00:00"),
                Title = "В тренажерном зале новая девушка на рецепшне - просто чистый лист формата А4",
                Text = "- Я: (выходя из зала) дайте пожалуйста маленькое полотенце<br />- Д: маленькое полотенце положено клиентам приходящим не менее 4-х раз в месяц<br />- Я: все верно, у меня график - раз в неделю<br />- Д: в компьютере зарегистрировано только одно ваше посещение в этом месяце<br />- Я: ну так сегодня 2 декабря, я первый раз пришел<br />- Д: (мило улыбаясь) ничем не могу помочь<br />- Я: (слегка ворча) вы, видимо, здесь не на долго<br />- Д: (удивленно) да, я только на зиму устроилась, как вы узнали?!",
                User = admin
            };
            Posts post2 = new Posts() {
                Date = DateTime.Parse("02.01.2015 14:00:00"),
                Title = "Были времена...",
                Text = "@Img(1)",
                User = editor
            };
            Posts post3 = new Posts() {
                Date = DateTime.Parse("03.01.2015 21:00:00"),
                Title = "Как отличить воробья от вороны",
                Text = "Первые попытки записать на катушечный магнитофон голоса певчих птиц мной были предприняты лет в 10-11. Писал что-то сам, что-то, зная увлечение дарили, что-то где-то выискивал, всё складывалось в копилку. С появлением компьютеров лента оцифровывалась, даже научился обрабатывать, шум леса и ветра убирать. Певчих птиц держал дома, было время, когда в квартире одномоментно жило до 200 птиц. Потом вдруг неожиданно старым стал. Слух потихоньку теряется, а с разрушением моей страны и появления свободной и независимой пропала и материальная возможность, птиц ведь кормить надо.<br />Папки лежат на компе мёртвым грузом, а ведь там записи, собранные, сейчас и не вспомнить где, когда, у кого и какая, за 40 лет. Детям на фиг не надо, у них свои интересы, внуки.<br /><a href='https://cloud.mail.ru/public/CGbS/8FTtrkpsB'>Ссылка</a>",
                User = admin
            };

            Comments comment1 = new Comments() {
                Date = DateTime.Parse("01.01.2015 14:00:00"),
                Comment = "А потом она поделит пиццу на 6 кусочков, потому что 12 - не съест :)",
                User = user
            };
            Comments comment2 = new Comments() {
                Date = DateTime.Parse("01.01.2015 21:00:00"),
                Comment = "А купив йогурт, откроет его в магазине, потому что на баночке написано 'открывать здесь'",
                User = editor
            };
            Comments comment3 = new Comments() {
                Date = DateTime.Parse("02.01.2015 07:00:00"),
                Comment = "Почему некоторые 2-х ядерные процессоры быстрее 4-х ядерников? А всё просто, пока 4-х ядерный раз-два-три-четыре раз-два-три-четыре, 2-х ядерник раз-два раз-два раз два.",
                User = admin
            };
            Comments comment4 = new Comments() {
                Date = DateTime.Parse("02.01.2015 21:00:00"),
                Comment = "Это было в лохматых годах... Друг качал игруху целую неделю, а после случайно оказалось, что такая игра была у знакомых на диске. Ну вы понимаете взрыв пукана был виден из дальнего космоса.",
                User = editor
            };
            Comments comment5 = new Comments() {
                Date = DateTime.Parse("04.01.2015 07:00:00"),
                Comment = "Дружище! Работаю театральным звукарём. От всей души благодарю тебя! Пригодится, не сомневайся!",
                User = user
            };
            Comments comment6 = new Comments() {
                Date = DateTime.Parse("04.01.2015 14:00:00"),
                Comment = "Можно конвертнуть в .ogg и добавить в соответствующие статьи википедии, тогда точно будет польза.",
                User = user
            };

            Images img1 = new Images() {
                Name = "/Images/1.jpg"
            };

            Tags tag1 = new Tags() {
                Name = "Качалка"
            };
            Tags tag2 = new Tags() {
                Name = "Мне не дали"
            };
            Tags tag3 = new Tags() {
                Name = "Птицы"
            };
            Tags tag4 = new Tags() {
                Name = "Текст"
            };

            // Связь один ко многим (post -> comments)
            post1.Comments.Add(comment1);
            post1.Comments.Add(comment2);
            post1.Comments.Add(comment3);
            post2.Comments.Add(comment4);
            post3.Comments.Add(comment5);
            post3.Comments.Add(comment6);

            // Связь многие ко многим (posts <-> tags)
            post2.Images.Add(img1);
            post1.Tags.Add(tag1);
            post1.Tags.Add(tag2);
            post1.Tags.Add(tag4);
            post3.Tags.Add(tag3);
            post3.Tags.Add(tag4);

            context.Posts.AddRange(new Posts[] { post1, post2, post3 });
            context.Comments.AddRange(new Comments[] { comment1, comment2, comment3, comment4, comment5, comment6 });
            context.Images.AddRange(new Images[] { img1 });
            context.Tags.AddRange(new Tags[] { tag1, tag2, tag3, tag4 });
            #endregion

            // Сохранить изменения.
            context.SaveChanges();
        }
    }
}