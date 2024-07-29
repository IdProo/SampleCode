package dev.zeroplus.models

import com.google.gson.annotations.SerializedName

data class Position (

    @SerializedName("id") val id : String,
    @SerializedName("name") val name : String,
    @SerializedName("organization") val organization : Organization,
    @SerializedName("kbo") val kbo : String,
    @SerializedName("lastModified") val lastModified : String,
    @SerializedName("isPublished") val isPublished : Boolean
)