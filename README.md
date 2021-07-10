# IdentityServer4.Demo



> 参考 [IdentityServer4中文文档](http://www.identityserver.com.cn/)



## 客户端凭证

+ IdentityServer  认证服务器
+ ResourceApp  访问的资源
+ ClientApp   客户端

## 密码模式

+ IdentityServer  认证服务器
+ PasswordApp 客户端

## Implicit模式

+ IdentityServer  认证服务器
+ OpenIdConnect 客户端

```csharp
// Implicit
options.ClientId = "im.client";
options.ClientSecret = "secret";
```

## 授权码模式

+ IdentityServer  认证服务器
+ OpenIdConnect 客户端

```csharp
// 授权码
options.ClientId = "co.client";
options.ClientSecret = "secret";
options.ResponseType = "code";
```

## HyBrid模式

+ IdentityServer  认证服务器
+ OpenIdConnect 客户端

```csharp
// Hybrid
options.ClientId = "hy.client";
options.ClientSecret = "secret";
options.ResponseType = "code id_token token";

options.SaveTokens = true;
options.GetClaimsFromUserInfoEndpoint = true;

options.Scope.Add("offline_access");
```

## js客户端

+ IdentityServer  认证服务器
+ JsClient  客户端