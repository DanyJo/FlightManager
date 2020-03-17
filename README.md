1) Преди първото пускане на проекта от HomeController-a трябва да се отзакоментират закоментираниете части код, за да могат да се
създадат ролите admin и user
2) Във Views/Shared/Layout - 
  "<li class="nav-item">
      <a class="nav-link text-white" asp-area="Identity" asp-page="/Account/Register">Create user</a>           
  </li>"
трябва да се издвади от if-а, за да може да се създаде първият потребител, който ще бъде админ, и после може да се върне обратно
