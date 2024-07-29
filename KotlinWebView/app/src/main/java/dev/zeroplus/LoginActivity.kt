package dev.zeroplus

import android.content.Intent
import android.content.SharedPreferences
import android.os.Bundle
import android.util.Log
import android.webkit.WebView
import android.webkit.WebViewClient
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.RequestQueue
import com.android.volley.Response
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import com.auth0.android.jwt.Claim
import com.auth0.android.jwt.JWT
import com.google.gson.Gson
import dev.zeroplus.models.User
import org.json.JSONException
import org.json.JSONObject
import java.nio.charset.Charset
import java.util.*

class LoginActivity : AppCompatActivity() {
    private lateinit var webView: WebView

    val loginIssuer: String = "https://login.issuer.com/"
    val clientId: String = "client_id"
    val secret: String = "client_secret"
    val redirectUri: String = "com.package.app.auth:/oauthredirect"
    val scopes: Array<String> =
        arrayOf("openid", "profile", "email", "api.auth", "user.role", "user.read")
    var shared: SharedPreferences? = null
    val idProoRestUri = "https://rest.com/"

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)
        shared = getSharedPreferences("office", 0)
        webView = findViewById(R.id.wvLogin)
        webView.settings.setJavaScriptEnabled(true)

        webView.webViewClient = object : WebViewClient() {
            override fun shouldOverrideUrlLoading(view: WebView?, url: String?): Boolean {
                if (url!!.startsWith(redirectUri)) {
                    authenticateUser(url!!)
                    return true
                } else {
                    return false
                }
            }
        }
        val strScopes: String = scopes.joinToString(" ")
        val nonce: String = UUID.randomUUID().toString()
        webView.loadUrl("${loginIssuer}connect/authorize/callback?client_id=${clientId}&response_type=code id_token&scope=${strScopes}&redirect_uri=${redirectUri}&state=axy&nonce=${nonce}")
    }

    fun authenticateUser(url: String) {
        Log.d("User Token", "$url")
        val params: List<String> = url.replace("${redirectUri}#", "").split("&")

        val authsParams = LinkedHashMap<String, String>()
        for (p in params) {
            val data = p.split("=");
            authsParams["${data.first()}"] = data.last()
        }

        Log.d("authsParams", "$authsParams")
        Log.d("code", "${authsParams["code"]}")

        val code: String = authsParams["code"].toString()
        val queue: RequestQueue = Volley.newRequestQueue(this)
        val url = "${loginIssuer}connect/token"

        val requestBody =
            "client_id=$clientId&client_secret=$secret&grant_type=authorization_code&scope=${
                scopes.joinToString(" ")
            }&code=$code&redirect_uri=$redirectUri"
        val stringReq: StringRequest =
            object : StringRequest(Method.POST, url,
                Response.Listener { response ->
                    // response
                    var strResp = response.toString()
                    val result: JSONObject = JSONObject(strResp)
                    val jwt: JWT = JWT(result["access_token"].toString());
                    val email = (jwt.claims["email"] as Claim).asString()
                    val name = (jwt.claims["display_name"] as Claim).asString()
                    getUser(queue, email!!, result["access_token"].toString())
                    if (shared is SharedPreferences) {

                        val editor: SharedPreferences.Editor = shared!!.edit()
                        editor.putString("accessToken", result["access_token"].toString())
                        editor.apply()
                    }

                },
                Response.ErrorListener { error ->
                    Log.d("API", "error => $error")
                }
            ) {
                override fun getBody(): ByteArray {
                    return requestBody.toByteArray(Charset.defaultCharset())
                }
            }
        queue.add(stringReq)


    }


    fun getUser(requestQueue: RequestQueue, email: String, bearer: String) {
        try {
            val request = object : StringRequest(
                Request.Method.GET,
                "${idProoRestUri}v1/Users/$email",
                { response ->
                    val user: User = Gson().fromJson(response, User::class.java)
                    val strUser = Gson().toJson(user)
                    if (shared is SharedPreferences) {
                        val editor: SharedPreferences.Editor = shared!!.edit()
                        editor.putString("userId", user.userId)
                        editor.putString("displayName", user.displayName)
                        editor.putString("email", user.email)
                        editor.putString("user", strUser)
                        editor.apply()

                        val intent = Intent(this, HomeActivity::class.java)
                        intent.flags =
                            Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                        startActivity(intent)
                        finish()
                    } else {
                        val intent = Intent(this, MainActivity::class.java)
                        intent.flags =
                            Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                        startActivity(intent)
                        finish()
                    }
                },
                { error ->
                    error.printStackTrace()
                }
            ) {
                override fun getHeaders(): MutableMap<String, String> {
                    val headers = HashMap<String, String>()
                    headers["Authorization"] = "Bearer $bearer"
                    return headers
                }
            }
            requestQueue?.add(request)
        } catch (e: Exception) {
            e.printStackTrace()
        }
    }


}