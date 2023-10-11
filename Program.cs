using InterfaceProxy;

IPayment paymentProxy = RemotingServiceFactory.GetService<IPayment>();

Basket basket = new ();
basket.AddProduct(new Product { Name = "Bananas", Quantity = 10 });

Payment response = await paymentProxy.Checkout(basket);