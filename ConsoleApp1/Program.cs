using ConsoleApp1.Services;
using Library.eCommerce.Services;

var productService = ProductServiceProxy.Current;
var cartService = new CartService(productService);
var commerceService = new CommerceService(productService, cartService);
var consoleService = new ConsoleCommerceService(commerceService);

consoleService.Run();