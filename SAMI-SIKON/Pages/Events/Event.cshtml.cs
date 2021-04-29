using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SAMI_SIKON.Pages.Events {
    public class EventModel : PageModel {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Theme { get; set; }
        public string PictureSrc { get; set; }
        public string PictureAlt { get; set; }
        public int Seats { get; set; }

        public EventModel() {
            Name = "Test Event";
            Theme = "Testing of algorithmical logic and aesthetically designed pages";
            Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum. Typi non habent claritatem insitam; est usus legentis in iis qui facit eorum claritatem. Investigationes demonstraverunt lectores legere me lius quod ii legunt saepius. Claritas est etiam processus dynamicus, qui sequitur mutationem consuetudium lectorum. Mirum est notare quam littera gothica, quam nunc putamus parum claram, anteposuerit litterarum formas humanitatis per seacula quarta decima et quinta decima. Eodem modo typi, qui nunc nobis videntur parum clari, fiant sollemnes in futurum.";
            PictureSrc = "/pictures/Auditorium.jpeg";
            PictureAlt = "Auditorium";
            Seats = 123;
        }

        /// <summary>
        /// Temp, please delete
        /// </summary>
        public void OnGet() {

        }

        //public void OnGet(int id) {

        //}
    }
}
