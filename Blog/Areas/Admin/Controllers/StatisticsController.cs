using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Blog.DAL;
using Blog.Areas.Admin.Models;
using Blog.Models;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class StatisticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Статистика просмотров страниц на сайте.
        /// Если пользователь откроет страницу, а потом перезагрузит ее, система зарегистрирует два просмотра.
        /// В рамках одного сеанса может быть зарегистрирован только один уникальный просмотр для каждой страницы.
        /// </summary>
        /// <param name="days">
        /// Количество дней, за которые показывать статистику.
        /// <list type="bullet">
        ///     <item><term>null</term><description>За все время.</description></item>
        ///     <item><term>0+</term><description>Стандартное поведение.</description></item>
        /// </list>
        /// </param>
        public ActionResult Visits(int? days)
        {
            List<CountVisitsViewModel> viewModel = new List<CountVisitsViewModel>();

            DateTime endDate = DateTime.Today.AddDays(1);
            DateTime startDate = (days > 0) ? DateTime.Today.AddDays(-((int)days) + 1) : DateTime.Parse("01.01.1970 00:00:00");
            IQueryable<Visits> between = db.Visits.Where(v => v.Date > startDate && v.Date < endDate);

            List<IGrouping<string, Visits>> visitedPages = between.GroupBy(v => v.Url).ToList();
            foreach (var visits in visitedPages) {
                string url = visits.Key;

                // 1 сеанс - 1 уникальный просмотр.
                int uniqueVisits = visits.GroupBy(v => v.Sessions).Count();

                viewModel.Add(new CountVisitsViewModel {
                    Url = url,
                    CountVisits = visits.Count(),
                    CountUniqueVisits = uniqueVisits
                });
            }

            return View(viewModel);
        }

        /// <summary>
        /// Показывает число уникальных пользователей на сайте.
        /// </summary>
        public ActionResult Sessions()
        {
            CountSessionsOnTimeViewModel viewModel = new CountSessionsOnTimeViewModel();
            return View(viewModel);
        }
    }
}