import os
import joblib

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel


MODEL_PATH = "models/models.joblib"


app = FastAPI(
    title="SmartSupport ML Service",
    version="1.0.0"
)


class PredictionRequest(BaseModel):
    text: str


if not os.path.exists(MODEL_PATH):
    raise RuntimeError(
        "ML model not found. Run 'python train.py' first."
    )


models = joblib.load(MODEL_PATH)


@app.get("/health")
def health():
    return {
        "status": "healthy",
        "model_loaded": True
    }


@app.post("/predict")
def predict(request: PredictionRequest):
    text = request.text.strip()

    if not text:
        raise HTTPException(
            status_code=400,
            detail="Ticket text cannot be empty."
        )

    category_model = models["category"]
    priority_model = models["priority"]
    sentiment_model = models["sentiment"]

    category = category_model.predict([text])[0]
    priority = priority_model.predict([text])[0]
    sentiment = sentiment_model.predict([text])[0]

    category_confidence = max(
        category_model.predict_proba([text])[0]
    )

    priority_confidence = max(
        priority_model.predict_proba([text])[0]
    )

    sentiment_confidence = max(
        sentiment_model.predict_proba([text])[0]
    )

    confidence = (
        category_confidence
        + priority_confidence
        + sentiment_confidence
    ) / 3

    return {
        "category": category,
        "priority": priority,
        "sentiment": sentiment,
        "confidence": round(float(confidence), 2)
    }
