package dev.zeroplus

import android.content.Intent
import android.content.SharedPreferences
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.Handler

class SplashActivity : AppCompatActivity() {
    var shared: SharedPreferences?=null;
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_splash)
        shared = getSharedPreferences("office", 0)

        Handler().postDelayed({
            var intent =  Intent(this, MainActivity::class.java)
            if (shared!!.getString("user",null) != null){
                intent = Intent(this, HomeActivity::class.java)
            }
            startActivity(intent)
            finish()
        }, 1500)
    }
}