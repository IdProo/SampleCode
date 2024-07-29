package dev.zeroplus.models

import com.google.gson.annotations.SerializedName

data class Organization(

    @SerializedName("id") val id: String,
    @SerializedName("name") val name: String,
    @SerializedName("companyCode") val companyCode: String
)