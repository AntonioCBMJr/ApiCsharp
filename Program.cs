using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World! alterado 2");
//app.MapGet("/user", () => "Antonio Carlos");
app.MapPost("/", () => new {Name = "Antonio Carlos", Age = 25});
app.MapGet("/AddHeader", (HttpResponse response) => {
    response.Headers.Add("Teste", "Antonio Carlos");
    return "Olá";
    });

//API.APP.COM/USERS?DATASTART={DATA}&DATEEND={DATE}
//http://localhost:5225/getproduct?datestart=x1&dateend=y2
app.MapGet("/getproduct", ([FromQuery]string dateStart, [FromQuery]string dateEnd) => {
    return dateStart + " - " + dateEnd;
});

app.MapPost("/products", (Product product) => {
    //return product.Code + " - " + product.Name;
    ProductRepository.Add(product);
    return Results.Created("/products/" + product.Code, product.Code);

});

//http://localhost:5225/getproduct/xptto
app.MapGet("/products/{code}", ([FromRoute]string code) => {
    //return code;
    var product = ProductRepository.GetBy(code);
    return product;
});

app.MapPut("/products", (Product product) => {
    var productSevad = ProductRepository.GetBy(product.Code);
    productSevad.Name = product.Name;
});

app.MapDelete("/products/{code}", ([FromRoute]string code) => {
    var productSevad = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSevad);
});


app.MapGet("/getproductbyheader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();

});

app.Run();

public static class ProductRepository{
    public static List<Product> Products { get; set; }

    public static void Add(Product product) {
        if(Products == null)
            Products = new List<Product>();

        Products.Add(product);
    }

    public static Product GetBy(string code) {
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product) {
        Products.Remove(product);
    }
}


public class Product {
    public string Code {get;set;}

    public string Name {get;set;}
}
