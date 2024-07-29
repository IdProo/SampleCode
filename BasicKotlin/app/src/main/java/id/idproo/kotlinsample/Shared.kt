package id.idproo.kotlinsample

object Shared {
    val oauth: OAuth = OAuth
}


object OAuth {
    const val issuer: String = "https://login.issuer.com"
    const val clientId: String = "xxxx"
    const val redirect: String = "id.idproo.sample:/oauthredirect"
    const val endSessionRedirect: String = "id.idproo.sample:/"
}