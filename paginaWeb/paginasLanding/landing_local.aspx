<%@ Page Title="" Language="C#" MasterPageFile="~/paginasLanding/landing_local.Master" AutoEventWireup="true" CodeBehind="landing_local.aspx.cs" Inherits="paginaWeb.paginasLanding.landing_local1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <main class="main">

      <!-- Hero Section -->
      <section id="hero" class="hero section">
          <div class="hero-bg">
              <img src="/assets/landing_local/assets/img/hero-bg-light.webp" alt="">
          </div>
          <div class="container text-center">
              <div class="d-flex flex-column justify-content-center align-items-center">
                  <h1 data-aos="fade-up">Bienvenido a <span>Shami SICAS</span></h1>
                  <p data-aos="fade-up" data-aos-delay="100">Esta página está desarrollada para ayudarte y mantenerte informado sobre las ultimas novedades de Shami.<br></p>
                  <div class="d-flex" data-aos="fade-up" data-aos-delay="200">
                      <a href="#about" class="btn-get-started">Ultimas Noticias</a>
                      <a href="https://www.youtube.com/watch?v=Cy_SiA-xhpY" class="glightbox btn-watch-video d-flex align-items-center"><i class="bi bi-play-circle"></i><span>Watch Video</span></a>
                  </div>
                  <img src="/assets/landing_local/assets/img/hero-services-img.gif" class="img-fluid hero-img" alt="" data-aos="zoom-out" data-aos-delay="300">

              </div>
          </div>

      </section><!-- /Hero Section -->
      <!-- Featured Services Section -->
      <section id="featured-services" class="featured-services section light-background">

          <div class="container">

              <div class="row gy-4">

                  <div class="col-xl-4 col-lg-6" data-aos="fade-up" data-aos-delay="100">
                      <div class="service-item d-flex">
                          <div class="icon flex-shrink-0"><i class="bi bi-briefcase"></i></div>
                          <div>
                              <h4 class="title"><a href="#" class="stretched-link">Pedidos</a></h4>
                              <p class="description">En Shami SICAS puedes realizar los pedidos de tu local de una manera sencilla y rapida</p>
                          </div>
                      </div>
                  </div>
                  <!-- End Service Item -->

                  <div class="col-xl-4 col-lg-6" data-aos="fade-up" data-aos-delay="200">
                      <div class="service-item d-flex">
                          <div class="icon flex-shrink-0"><i class="bi bi-card-checklist"></i></div>
                          <div>
                              <h4 class="title"><a href="#" class="stretched-link">Administracion de Empleados</a></h4>
                              <p class="description">En Shami SICAS puedes administrar los datos y cargos de tus empleados</p>
                          </div>
                      </div>
                  </div><!-- End Service Item -->

                  <div class="col-xl-4 col-lg-6" data-aos="fade-up" data-aos-delay="300">
                      <div class="service-item d-flex">
                          <div class="icon flex-shrink-0"><i class="bi bi-bar-chart"></i></div>
                          <div>
                              <h4 class="title"><a href="#" class="stretched-link">Estadistica de Pedidos</a></h4>
                              <p class="description">En Shami SICAS puedes realizar una consulta de la estadisticas de tus pedidos</p>
                          </div>
                      </div>
                  </div><!-- End Service Item -->

              </div>

          </div>

      </section><!-- /Featured Services Section -->
      <!-- About Section -->
      <section id="about" class="about section">

          <div class="container">

              <div class="row gy-4">

                  <div class="col-lg-6 content" data-aos="fade-up" data-aos-delay="100">
                      <p class="who-we-are">NOVEDADES</p>
                      <h3>Ultimas Novedades</h3>
                      <p class="fst-italic">
                          A continuacion encontraras un resumen de las ultimas novedades.
                      </p>
                      <ul>
                          <li><i class="bi bi-check-circle"></i> <span>Cambiamos la forma de nuestro Mamul de Datiles.</span></li>
                          <li><i class="bi bi-check-circle"></i> <span>Se agregaron nuevos modulos al Sistema.</span></li>
                          <li><i class="bi bi-check-circle"></i> <span>Proximamente estará disponible un nuevo Modulo de Capacitaciones.</span></li>
                      </ul>
                      <a href="/paginasLanding/landing_local.html#features" class="read-more"><span>Saber Mas</span><i class="bi bi-arrow-right"></i></a>
                  </div>

                  <div class="col-lg-6 about-images" data-aos="fade-up" data-aos-delay="200">
                      <div class="row gy-4">
                          <div class="col-lg-6">
                              <img src="/assets/landing_local/assets/img/nueva-forma.jpg" class="img-fluid" alt="">
                          </div>
                          <div class="col-lg-6">
                              <div class="row gy-4">
                                  <div class="col-lg-12">
                                      <img src="/assets/landing_local/assets/img/novedad-sistema.jpg" class="img-fluid" alt="">
                                  </div>
                                  <div class="col-lg-12">
                                      <img src="/assets/landing_local/assets/img/estamos-trabajando.jpg" class="img-fluid" alt="">
                                  </div>
                              </div>
                          </div>
                      </div>

                  </div>

              </div>

          </div>
      </section><!-- /About Section -->
      <!-- Clients Section -->
      <!-- <section id="clients" class="clients section">

      <div class="container" data-aos="fade-up">

          <div class="row gy-4">

              <div class="col-xl-2 col-md-3 col-6 client-logo">
                  <img src="/assets/landing_local/assets/img/clients/client-1.png" class="img-fluid" alt="">
              </div><!-- End Client Item -->
      <!--  <div class="col-xl-2 col-md-3 col-6 client-logo">
          <img src="/assets/landing_local/assets/img/clients/client-2.png" class="img-fluid" alt="">
      </div><!-- End Client Item -->
      <!--  <div class="col-xl-2 col-md-3 col-6 client-logo">
          <img src="/assets/landing_local/assets/img/clients/client-3.png" class="img-fluid" alt="">
      </div><!-- End Client Item -->
      <!--   <div class="col-xl-2 col-md-3 col-6 client-logo">
          <img src="/assets/landing_local/assets/img/clients/client-4.png" class="img-fluid" alt="">
      </div><!-- End Client Item -->
      <!--  <div class="col-xl-2 col-md-3 col-6 client-logo">
          <img src="/assets/landing_local/assets/img/clients/client-5.png" class="img-fluid" alt="">
      </div><!-- End Client Item -->
      <!--   <div class="col-xl-2 col-md-3 col-6 client-logo">
          <img src="/assets/landing_local/assets/img/clients/client-6.png" class="img-fluid" alt="">
      </div><!-- End Client Item -->
      <!--     </div>

      </div>

      </section><!-- /Clients Section -->
      <!-- Features Section -->
      <section id="features" class="features section">

          <!-- Section Title -->
          <div class="container section-title" data-aos="fade-up">
              <h2>Novedades</h2>
              <p>Acá encontrarás detalles de las últimas novedades.</p>
          </div><!-- End Section Title -->

          <div class="container">
              <div class="row justify-content-between">

                  <div class="col-lg-5 d-flex align-items-center">

                      <ul class="nav nav-tabs" data-aos="fade-up" data-aos-delay="100">
                          <li class="nav-item">
                              <a class="nav-link active show" data-bs-toggle="tab" data-bs-target="#features-tab-1">
                                  <i class="bi bi-cookie"></i>
                                  <div>
                                      <h4 class="d-none d-lg-block">Cambiamos la forma de nuestro Mamul de Datiles.</h4>
                                      <p>
                                          Ahora nuestro Mamul de Datiles tiene una forma mas atractiva y facil de comer.
                                      </p>
                                  </div>
                              </a>
                          </li>
                          <li class="nav-item">
                              <a class="nav-link" data-bs-toggle="tab" data-bs-target="#features-tab-2">
                                  <i class="bi bi-clipboard-data"></i>
                                  <div>
                                      <h4 class="d-none d-lg-block">Se agregaron nuevos modulos al Sistema.</h4>
                                      <p>Administracion del Local: Aqui podrá registrar las ventas de su local.</p>
                                      <p>Operaciones del Local: Podrás llevar a cabo todo el registro de operaciones de tu local.</p>
                                      <p>Capacitacion: Vas a encontrar disponible diferentes tipos de capacitaciones.</p>
                                  </div>
                              </a>
                          </li>
                          <li class="nav-item">
                              <a class="nav-link" data-bs-toggle="tab" data-bs-target="#features-tab-3">
                                  <i class="bi bi-brightness-high"></i>
                                  <div>
                                      <h4 class="d-none d-lg-block">Proximamente estará disponible un nuevo Modulo de Capacitaciones.</h4>
                                      <p>
                                          Estamos trabajando en un nuevo modulo de capacitaciones donde encontraras fichas técnicas y tutoriales de preparacion de todos nuestros productos Shami.
                                      </p>
                                  </div>
                              </a>
                          </li>
                      </ul><!-- End Tab Nav -->

                  </div>

                  <div class="col-lg-6">

                      <div class="tab-content" data-aos="fade-up" data-aos-delay="200">

                          <div class="tab-pane fade active show" id="features-tab-1">
                              <img src="/assets/landing_local/assets/img/mamul-datiles-nueva-forma.jpg" alt="" class="img-fluid">
                          </div><!-- End Tab Content Item -->

                          <div class="tab-pane fade" id="features-tab-2">
                              <img src="/assets/landing_local/assets/img/novedad-sistema.jpg" alt="" class="img-fluid">
                          </div><!-- End Tab Content Item -->

                          <div class="tab-pane fade" id="features-tab-3">
                              <img src="/assets/landing_local/assets/img/estamos-trabajando.jpg" alt="" class="img-fluid">
                          </div><!-- End Tab Content Item -->
                      </div>

                  </div>

              </div>

          </div>

      </section><!-- /Features Section -->
      <!-- Features Details Section -->
      <section id="features-details" class="features-details section">

          <div class="container">

              <div class="row gy-4 justify-content-between features-item">

                  <div class="col-lg-6" data-aos="fade-up" data-aos-delay="100">
                      <img src="/assets/landing_local/assets/img/gestiona-desde-celular.jpg" class="img-fluid" alt="">
                  </div>

                  <div class="col-lg-5 d-flex align-items-center" data-aos="fade-up" data-aos-delay="200">
                      <div class="content">
                          <h3>GESTION EN LA PALMA DE TU MANO.</h3>
                          <p>
                              Puedes realizar todas las gestiones desde tu teléfono celular.
                          </p>
                          <!-- <a href="#" class="btn more-btn">Learn More</a>-->
                      </div>
                  </div>

              </div><!-- Features Item -->
              <!--   <div class="row gy-4 justify-content-between features-item">

                  <div class="col-lg-5 d-flex align-items-center order-2 order-lg-1" data-aos="fade-up" data-aos-delay="100">

                      <div class="content">
                          <h3>Neque ipsum omnis sapiente quod quia dicta</h3>
                          <p>
                              Quidem qui dolore incidunt aut. In assumenda harum id iusto lorena plasico mares
                          </p>
                          <ul>
                              <li><i class="bi bi-easel flex-shrink-0"></i> Et corporis ea eveniet ducimus.</li>
                              <li><i class="bi bi-patch-check flex-shrink-0"></i> Exercitationem dolorem sapiente.</li>
                              <li><i class="bi bi-brightness-high flex-shrink-0"></i> Veniam quia modi magnam.</li>
                          </ul>
                          <p></p>
                          <a href="#" class="btn more-btn">Learn More</a>
                      </div>

                  </div>

                  <div class="col-lg-6 order-1 order-lg-2" data-aos="fade-up" data-aos-delay="200">
                      <img src="/assets/landing_local/assets/img/features-2.jpg" class="img-fluid" alt="">
                  </div>

              </div><!-- Features Item -->

          </div>

      </section><!-- /Features Details Section -->
      <!-- Services Section -->
      <section id="services" class="services section light-background">

          <!-- Section Title -->
          <div class="container section-title" data-aos="fade-up">
              <h2>Modulos Agregados</h2>
              <p>A continuación se detallan los últimos módulos agregados al sistema.</p>
          </div><!-- End Section Title -->

          <div class="container">

              <div class="row g-5">

                  <div class="col-lg-6" data-aos="fade-up" data-aos-delay="100">
                      <div class="service-item item-cyan position-relative">
                          <i class="bi bi-clipboard-data icon"></i>
                          <div>
                              <h3>Registro de Ventas del Local</h3>
                              <p>En la categoria "Administracion del Local" encontrarás el modulo "Registro de Ventas del Local". Ahora cada cajero debe registrar las ventas de su Turno.</p>
                              <!--<a href="#" class="read-more stretched-link">Learn More <i class="bi bi-arrow-right"></i></a>-->
                          </div>
                      </div>
                  </div><!-- End Service Item -->

                  <div class="col-lg-6" data-aos="fade-up" data-aos-delay="200">
                      <div class="service-item item-orange position-relative">
                          <i class="bi bi-person-add icon"></i>
                          <div>
                              <h3>Administrar Empleados</h3>
                              <p>En la categoria "Operaciones del Local" encontrarás el modulo "Administrar Empleados".</p>
                              <p>Acá podrás ingresar los empleados de tu local, administrar sus datos y responsabilidades.</p>
                              <!--<a href="#" class="read-more stretched-link">Learn More <i class="bi bi-arrow-right"></i></a>-->

                          </div>
                      </div>
                  </div><!-- End Service Item -->

                  <div class="col-lg-6" data-aos="fade-up" data-aos-delay="300">
                      <div class="service-item item-teal position-relative">
                          <i class="bi bi-apple icon"></i>
                          <div>
                              <h3>Comida de Empleados</h3>
                              <p>En la categoria "Operaciones del Local" encontrarás el modulo "Comida de Empleados".</p>
                              <p>Acá podrás ingresar el consumo diario de los empleados de tu local.</p>
                              <!--<a href="#" class="read-more stretched-link">Learn More <i class="bi bi-arrow-right"></i></a>-->
                          </div>
                      </div>
                  </div><!-- End Service Item -->

                  <div class="col-lg-6" data-aos="fade-up" data-aos-delay="400">
                      <div class="service-item item-red position-relative">
                          <i class="bi bi-card-checklist icon"></i>
                          <div>
                              <h3>Historial de Listas de Chequeo</h3>
                              <p>En la categoria "Operaciones del Local" encontrarás el modulo "Historial de Listas de Chequeo".</p>
                              <p>Acá podrás visualizar los empleados logueados segun fecha y turno y ver el detalle de cumplimiento de su lista de chequeo.</p>
                              <!--<a href="#" class="read-more stretched-link">Learn More <i class="bi bi-arrow-right"></i></a>-->
                          </div>
                      </div>
                  </div><!-- End Service Item -->

                  <div class="col-lg-6" data-aos="fade-up" data-aos-delay="500">
                      <div class="service-item item-indigo position-relative">
                          <i class="bi bi-calendar4-week icon"></i>
                          <div>
                              <h3>Administrar Tabla de Produccion</h3>
                              <p>En la categoria "Operaciones del Local" encontrarás el modulo "Administrar Tabla de Produccion".</p>
                              <p>Acá podrás seleccionar los dias de alta y baja venta e ingresar las unidades de productos vendidos por caja registradora, para que el sistema calcule la planificacion de produccion por turno segun los datos ingresados.</p>
                              <!--<a href="#" class="read-more stretched-link">Learn More <i class="bi bi-arrow-right"></i></a>-->
                          </div>
                      </div>
                  </div><!-- End Service Item -->

                  <div class="col-lg-6" data-aos="fade-up" data-aos-delay="600">
                      <div class="service-item item-pink position-relative">
                          <i class="bi bi-list-ol icon"></i>
                          <div>
                              <h3>Lista de Faltantes</h3>
                              <p>En la categoria "Operaciones del Local" encontrarás el modulo "Lista de Faltantes".</p>
                              <p>Acá cada empleado podrá marcar los productos con stock critico para un plazo de dos dias y generar un listado que podrá ser exportado en PDF a fin de poder ser enviado a su Encargado para la respectiva gestion de reposicion de productos.</p>
                              <!--<a href="#" class="read-more stretched-link">Learn More <i class="bi bi-arrow-right"></i></a>-->
                          </div>
                      </div>
                  </div><!-- End Service Item -->

              </div>

          </div>

      </section><!-- /Services Section -->
      <!-- More Features Section -->
      <section id="more-features" class="more-features section">

          <div class="container">

              <div class="row justify-content-around gy-4">

                  <div class="col-lg-6 d-flex flex-column justify-content-center order-2 order-lg-1" data-aos="fade-up" data-aos-delay="100">
                      <h3>En Desarrollo</h3>
                      <p>Estamos trabajando arduamente en el desarrollo de nuevas funcionalidades que te serviran en la gestion de tu local.</p>

                      <div class="row">

                          <div class="col-lg-6 icon-box d-flex">
                              <i class="bi bi-cash-coin flex-shrink-0"></i>
                              <div>
                                  <h4>Modulo Caja Chica</h4>
                                  <p>Registro diario de gastos operativos y compras menores.</p>
                              </div>
                          </div><!-- End Icon Box -->

                          <div class="col-lg-6 icon-box d-flex">
                              <i class="bi bi-truck flex-shrink-0"></i>
                              <div>
                                  <h4>Modulo Proveedores Locales</h4>
                                  <p>Registro de compras diarias a proveedores locales y/o terceros.</p>
                              </div>
                          </div><!-- End Icon Box -->

                          <div class="col-lg-6 icon-box d-flex">
                              <i class="bi bi-bar-chart flex-shrink-0"></i>
                              <div>
                                  <h4>Modulo Estado de Resultados (Ganancias y Pérdidas Mensuales)</h4>
                                  <p>Registro mensual de gastos fijos, y visualizacion de estado de ganancia y perdida.</p>
                              </div>
                          </div><!-- End Icon Box -->

                          <div class="col-lg-6 icon-box d-flex">
                              <i class="bi bi-clipboard-plus flex-shrink-0"></i>
                              <div>
                                  <h4>Modulo Conteo y Valorizacion de Stock</h4>
                                  <p>Registro semanal de stock de mercaderia y su valorizacion.</p>
                              </div>
                          </div><!-- End Icon Box -->

                      </div>

                  </div>

                  <div class="features-image col-lg-5 order-1 order-lg-2" data-aos="fade-up" data-aos-delay="200">
                      <img src="/assets/landing_local/assets/img/trabajando.jpg" alt="">
                  </div>

              </div>

          </div>

      </section><!-- /More Features Section -->
      <!-- Pricing Section -->
      <!--      <section id="pricing" class="pricing section">

      <!-- Section Title -->
      <!--      <div class="container section-title" data-aos="fade-up">
          <h2>Pricing</h2>
          <p>Necessitatibus eius consequatur ex aliquid fuga eum quidem sint consectetur velit</p>
      </div><!-- End Section Title -->
      <!--     <div class="container">

      <div class="row gy-4">

          <div class="col-lg-4" data-aos="zoom-in" data-aos-delay="100">
              <div class="pricing-item">
                  <h3>Free Plan</h3>
                  <p class="description">Ullam mollitia quasi nobis soluta in voluptatum et sint palora dex strater</p>
                  <h4><sup>$</sup>0<span> / month</span></h4>
                  <a href="#" class="cta-btn">Start a free trial</a>
                  <p class="text-center small">No credit card required</p>
                  <ul>
                      <li><i class="bi bi-check"></i> <span>Quam adipiscing vitae proin</span></li>
                      <li><i class="bi bi-check"></i> <span>Nec feugiat nisl pretium</span></li>
                      <li><i class="bi bi-check"></i> <span>Nulla at volutpat diam uteera</span></li>
                      <li class="na"><i class="bi bi-x"></i> <span>Pharetra massa massa ultricies</span></li>
                      <li class="na"><i class="bi bi-x"></i> <span>Massa ultricies mi quis hendrerit</span></li>
                      <li class="na"><i class="bi bi-x"></i> <span>Voluptate id voluptas qui sed aperiam rerum</span></li>
                      <li class="na"><i class="bi bi-x"></i> <span>Iure nihil dolores recusandae odit voluptatibus</span></li>
                  </ul>
              </div>
          </div><!-- End Pricing Item -->
      <!--     <div class="col-lg-4" data-aos="zoom-in" data-aos-delay="200">
          <div class="pricing-item featured">
              <p class="popular">Popular</p>
              <h3>Business Plan</h3>
              <p class="description">Ullam mollitia quasi nobis soluta in voluptatum et sint palora dex strater</p>
              <h4><sup>$</sup>29<span> / month</span></h4>
              <a href="#" class="cta-btn">Start a free trial</a>
              <p class="text-center small">No credit card required</p>
              <ul>
                  <li><i class="bi bi-check"></i> <span>Quam adipiscing vitae proin</span></li>
                  <li><i class="bi bi-check"></i> <span>Nec feugiat nisl pretium</span></li>
                  <li><i class="bi bi-check"></i> <span>Nulla at volutpat diam uteera</span></li>
                  <li><i class="bi bi-check"></i> <span>Pharetra massa massa ultricies</span></li>
                  <li><i class="bi bi-check"></i> <span>Massa ultricies mi quis hendrerit</span></li>
                  <li><i class="bi bi-check"></i> <span>Voluptate id voluptas qui sed aperiam rerum</span></li>
                  <li class="na"><i class="bi bi-x"></i> <span>Iure nihil dolores recusandae odit voluptatibus</span></li>
              </ul>
          </div>
      </div><!-- End Pricing Item -->
      <!--   <div class="col-lg-4" data-aos="zoom-in" data-aos-delay="300">
          <div class="pricing-item">
              <h3>Developer Plan</h3>
              <p class="description">Ullam mollitia quasi nobis soluta in voluptatum et sint palora dex strater</p>
              <h4><sup>$</sup>49<span> / month</span></h4>
              <a href="#" class="cta-btn">Start a free trial</a>
              <p class="text-center small">No credit card required</p>
              <ul>
                  <li><i class="bi bi-check"></i> <span>Quam adipiscing vitae proin</span></li>
                  <li><i class="bi bi-check"></i> <span>Nec feugiat nisl pretium</span></li>
                  <li><i class="bi bi-check"></i> <span>Nulla at volutpat diam uteera</span></li>
                  <li><i class="bi bi-check"></i> <span>Pharetra massa massa ultricies</span></li>
                  <li><i class="bi bi-check"></i> <span>Massa ultricies mi quis hendrerit</span></li>
                  <li><i class="bi bi-check"></i> <span>Voluptate id voluptas qui sed aperiam rerum</span></li>
                  <li><i class="bi bi-check"></i> <span>Iure nihil dolores recusandae odit voluptatibus</span></li>
              </ul>
          </div>
      </div><!-- End Pricing Item -->
      <!--     </div>

      </div>

      </section><!-- /Pricing Section -->
      <!-- Faq Section -->
      <!-- <section id="faq" class="faq section">

      <!-- Section Title -->
      <!--   <div class="container section-title" data-aos="fade-up">
          <h2>Frequently Asked Questions</h2>
      </div><!-- End Section Title -->
      <!--     <div class="container">

      <div class="row justify-content-center">

          <div class="col-lg-10" data-aos="fade-up" data-aos-delay="100">

              <div class="faq-container">

                  <div class="faq-item faq-active">
                      <h3>Non consectetur a erat nam at lectus urna duis?</h3>
                      <div class="faq-content">
                          <p>Feugiat pretium nibh ipsum consequat. Tempus iaculis urna id volutpat lacus laoreet non curabitur gravida. Venenatis lectus magna fringilla urna porttitor rhoncus dolor purus non.</p>
                      </div>
                      <i class="faq-toggle bi bi-chevron-right"></i>
                  </div><!-- End Faq item-->
      <!--    <div class="faq-item">
          <h3>Feugiat scelerisque varius morbi enim nunc faucibus?</h3>
          <div class="faq-content">
              <p>Dolor sit amet consectetur adipiscing elit pellentesque habitant morbi. Id interdum velit laoreet id donec ultrices. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Est pellentesque elit ullamcorper dignissim. Mauris ultrices eros in cursus turpis massa tincidunt dui.</p>
          </div>
          <i class="faq-toggle bi bi-chevron-right"></i>
      </div><!-- End Faq item-->
      <!--      <div class="faq-item">
          <h3>Dolor sit amet consectetur adipiscing elit pellentesque?</h3>
          <div class="faq-content">
              <p>Eleifend mi in nulla posuere sollicitudin aliquam ultrices sagittis orci. Faucibus pulvinar elementum integer enim. Sem nulla pharetra diam sit amet nisl suscipit. Rutrum tellus pellentesque eu tincidunt. Lectus urna duis convallis convallis tellus. Urna molestie at elementum eu facilisis sed odio morbi quis</p>
          </div>
          <i class="faq-toggle bi bi-chevron-right"></i>
      </div><!-- End Faq item-->
      <!--  <div class="faq-item">
          <h3>Ac odio tempor orci dapibus. Aliquam eleifend mi in nulla?</h3>
          <div class="faq-content">
              <p>Dolor sit amet consectetur adipiscing elit pellentesque habitant morbi. Id interdum velit laoreet id donec ultrices. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Est pellentesque elit ullamcorper dignissim. Mauris ultrices eros in cursus turpis massa tincidunt dui.</p>
          </div>
          <i class="faq-toggle bi bi-chevron-right"></i>
      </div><!-- End Faq item-->
      <!--    <div class="faq-item">
          <h3>Tempus quam pellentesque nec nam aliquam sem et tortor?</h3>
          <div class="faq-content">
              <p>Molestie a iaculis at erat pellentesque adipiscing commodo. Dignissim suspendisse in est ante in. Nunc vel risus commodo viverra maecenas accumsan. Sit amet nisl suscipit adipiscing bibendum est. Purus gravida quis blandit turpis cursus in</p>
          </div>
          <i class="faq-toggle bi bi-chevron-right"></i>
      </div><!-- End Faq item-->
      <!--   <div class="faq-item">
          <h3>Perspiciatis quod quo quos nulla quo illum ullam?</h3>
          <div class="faq-content">
              <p>Enim ea facilis quaerat voluptas quidem et dolorem. Quis et consequatur non sed in suscipit sequi. Distinctio ipsam dolore et.</p>
          </div>
          <i class="faq-toggle bi bi-chevron-right"></i>
      </div><!-- End Faq item-->
      <!--   </div>

      </div><!-- End Faq Column-->
      <!--    </div>

      </div>

      </section><!-- /Faq Section -->
      <!-- Testimonials Section -->
      <!-- <section id="testimonials" class="testimonials section light-background">

      <!-- Section Title -->
      <!-- <div class="container section-title" data-aos="fade-up">
          <h2>Testimonials</h2>
          <p>Necessitatibus eius consequatur ex aliquid fuga eum quidem sint consectetur velit</p>
      </div><!-- End Section Title -->
      <!--   <div class="container" data-aos="fade-up" data-aos-delay="100">

      <div class="swiper init-swiper">
          <script type="application/json" class="swiper-config">
              {
                "loop": true,
                "speed": 600,
                "autoplay": {
                  "delay": 5000
                },
                "slidesPerView": "auto",
                "pagination": {
                  "el": ".swiper-pagination",
                  "type": "bullets",
                  "clickable": true
                },
                "breakpoints": {
                  "320": {
                    "slidesPerView": 1,
                    "spaceBetween": 40
                  },
                  "1200": {
                    "slidesPerView": 3,
                    "spaceBetween": 1
                  }
                }
              }
          </script>
          <div class="swiper-wrapper">

              <div class="swiper-slide">
                  <div class="testimonial-item">
                      <div class="stars">
                          <i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i>
                      </div>
                      <p>
                          Proin iaculis purus consequat sem cure digni ssim donec porttitora entum suscipit rhoncus. Accusantium quam, ultricies eget id, aliquam eget nibh et. Maecen aliquam, risus at semper.
                      </p>
                      <div class="profile mt-auto">
                          <img src="/assets/landing_local/assets/img/testimonials/testimonials-1.jpg" class="testimonial-img" alt="">
                          <h3>Saul Goodman</h3>
                          <h4>Ceo &amp; Founder</h4>
                      </div>
                  </div>
              </div><!-- End testimonial item -->
      <!--   <div class="swiper-slide">
          <div class="testimonial-item">
              <div class="stars">
                  <i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i>
              </div>
              <p>
                  Export tempor illum tamen malis malis eram quae irure esse labore quem cillum quid cillum eram malis quorum velit fore eram velit sunt aliqua noster fugiat irure amet legam anim culpa.
              </p>
              <div class="profile mt-auto">
                  <img src="/assets/landing_local/assets/img/testimonials/testimonials-2.jpg" class="testimonial-img" alt="">
                  <h3>Sara Wilsson</h3>
                  <h4>Designer</h4>
              </div>
          </div>
      </div><!-- End testimonial item -->
      <!--   <div class="swiper-slide">
          <div class="testimonial-item">
              <div class="stars">
                  <i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i>
              </div>
              <p>
                  Enim nisi quem export duis labore cillum quae magna enim sint quorum nulla quem veniam duis minim tempor labore quem eram duis noster aute amet eram fore quis sint minim.
              </p>
              <div class="profile mt-auto">
                  <img src="/assets/landing_local/assets/img/testimonials/testimonials-3.jpg" class="testimonial-img" alt="">
                  <h3>Jena Karlis</h3>
                  <h4>Store Owner</h4>
              </div>
          </div>
      </div><!-- End testimonial item -->
      <!--     <div class="swiper-slide">
          <div class="testimonial-item">
              <div class="stars">
                  <i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i>
              </div>
              <p>
                  Fugiat enim eram quae cillum dolore dolor amet nulla culpa multos export minim fugiat minim velit minim dolor enim duis veniam ipsum anim magna sunt elit fore quem dolore labore illum veniam.
              </p>
              <div class="profile mt-auto">
                  <img src="/assets/landing_local/assets/img/testimonials/testimonials-4.jpg" class="testimonial-img" alt="">
                  <h3>Matt Brandon</h3>
                  <h4>Freelancer</h4>
              </div>
          </div>
      </div><!-- End testimonial item -->
      <!--     <div class="swiper-slide">
          <div class="testimonial-item">
              <div class="stars">
                  <i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i><i class="bi bi-star-fill"></i>
              </div>
              <p>
                  Quis quorum aliqua sint quem legam fore sunt eram irure aliqua veniam tempor noster veniam enim culpa labore duis sunt culpa nulla illum cillum fugiat legam esse veniam culpa fore nisi cillum quid.
              </p>
              <div class="profile mt-auto">
                  <img src="/assets/landing_local/assets/img/testimonials/testimonials-5.jpg" class="testimonial-img" alt="">
                  <h3>John Larson</h3>
                  <h4>Entrepreneur</h4>
              </div>
          </div>
      </div><!-- End testimonial item -->
      <!--    </div>
      <div class="swiper-pagination"></div>
      </div>

      </div>

      </section><!-- /Testimonials Section -->
      <!-- Contact Section -->
      <section id="contact" class="contact section">

          <!-- Section Title -->
          <div class="container section-title" data-aos="fade-up">
              <h2>Contacto</h2>
              <p>Ante consultas operativas, contacte con Asistencia Operativa. Ante problemas técnicos, contacte con Programacion.</p>
          </div><!-- End Section Title -->

          <div class="container" data-aos="fade-up" data-aos-delay="100">

              <div class="row gy-4">

                  <div class="col-lg-6">
                      <div class="info-item d-flex flex-column justify-content-center align-items-center" data-aos="fade-up" data-aos-delay="200">
                          <i class="bi bi-geo-alt"></i>
                          <h3>Direccion Oficina</h3>
                          <p>Superí 1409, C1426BAG Cdad. Autónoma de Buenos Aires</p>
                      </div>
                  </div><!-- End Info Item -->

                  <div class="col-lg-3 col-md-6">
                      <div class="info-item d-flex flex-column justify-content-center align-items-center" data-aos="fade-up" data-aos-delay="300">
                          <i class="bi bi-telephone"></i>
                          <h3>Asistencia Operativa</h3>
                          <a href="https://api.whatsapp.com/send?phone=5491168997755" target="_blank">Supervisor de Operaciones</a>
                      </div>
                  </div><!-- End Info Item -->

                  <div class="col-lg-3 col-md-6">
                      <div class="info-item d-flex flex-column justify-content-center align-items-center" data-aos="fade-up" data-aos-delay="300">
                          <i class="bi bi-telephone"></i>
                          <h3>Asistencia Tecnica</h3>
                          <a href="https://api.whatsapp.com/send?phone=5491160413980" target="_blank">Programador</a>
                      </div>
                  </div><!-- End Info Item -->
                  <!--  <div class="col-lg-3 col-md-6">
                      <div class="info-item d-flex flex-column justify-content-center align-items-center" data-aos="fade-up" data-aos-delay="400">
                          <i class="bi bi-envelope"></i>
                          <h3>Email Us</h3>
                          <p>info@example.com</p>
                      </div>
                  </div> End Info Item -->

              </div>

              <div class="row gy-4 mt-1">
                  <div class="col-lg-6" data-aos="fade-up" data-aos-delay="300">
                      <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3285.190775474343!2d-58.46260828777922!3d-34.57403905582016!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x95bcb5d920de2835%3A0x44642dbacdc54612!2sSuper%C3%AD%201409%2C%20C1426BAG%20Cdad.%20Aut%C3%B3noma%20de%20Buenos%20Aires!5e0!3m2!1ses-419!2sar!4v1722274350187!5m2!1ses-419!2sar" frameborder="0" style="border:0; width: 100%; height: 400px;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
                  </div><!-- End Google Maps -->
                  <div class="col-lg-6">
                      <div class="info-item d-flex flex-column justify-content-center align-items-center" data-aos="fade-up" data-aos-delay="300">
                          <i class="bi bi-telephone"></i>
                          <h3>Fabrica Shami</h3>
                          <a href="https://api.whatsapp.com/send?phone=5491128799999" target="_blank">Oficina Fabrica</a>
                      </div>
                  </div><!-- End Info Item -->
                  <!-- <div class="col-lg-6">
                      <form action="forms/contact.php" method="post" class="php-email-form" data-aos="fade-up" data-aos-delay="400">
                          <div class="row gy-4">

                              <div class="col-md-6">
                                  <input type="text" name="name" class="form-control" placeholder="Your Name" required="">
                              </div>

                              <div class="col-md-6 ">
                                  <input type="email" class="form-control" name="email" placeholder="Your Email" required="">
                              </div>

                              <div class="col-md-12">
                                  <input type="text" class="form-control" name="subject" placeholder="Subject" required="">
                              </div>

                              <div class="col-md-12">
                                  <textarea class="form-control" name="message" rows="6" placeholder="Message" required=""></textarea>
                              </div>

                              <div class="col-md-12 text-center">
                                  <div class="loading">Loading</div>
                                  <div class="error-message"></div>
                                  <div class="sent-message">Your message has been sent. Thank you!</div>

                                  <button type="submit">Send Message</button>
                              </div>

                          </div>
                      </form>
                  </div> End Contact Form -->

              </div>

          </div>

      </section><!-- /Contact Section -->

  </main>

  
</asp:Content>
