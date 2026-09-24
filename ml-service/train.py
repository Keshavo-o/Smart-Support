import os
import joblib
import pandas as pd

from sklearn.model_selection import train_test_split
from sklearn.pipeline import Pipeline
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score


DATA_PATH = "data/tickets.csv"
MODEL_DIR = "models"
MODEL_PATH = os.path.join(MODEL_DIR, "models.joblib")


def build_model():
    return Pipeline([
        (
            "tfidf",
            TfidfVectorizer(
                lowercase=True,
                ngram_range=(1, 2)
            )
        ),
        (
            "classifier",
            LogisticRegression(
                max_iter=1000
            )
        )
    ])


def train_target(df, target):
    X = df["text"]
    y = df[target]

    X_train, X_test, y_train, y_test = train_test_split(
        X,
        y,
        test_size=0.2,
        random_state=42,
        stratify=y
    )

    model = build_model()
    model.fit(X_train, y_train)

    predictions = model.predict(X_test)
    accuracy = accuracy_score(y_test, predictions)

    print(f"{target} accuracy: {accuracy:.2f}")

    return model


def main():
    if not os.path.exists(DATA_PATH):
        raise FileNotFoundError(
            f"Dataset not found: {DATA_PATH}"
        )

    os.makedirs(MODEL_DIR, exist_ok=True)

    df = pd.read_csv(DATA_PATH)

    required_columns = {
        "text",
        "category",
        "priority",
        "sentiment"
    }

    missing = required_columns - set(df.columns)

    if missing:
        raise ValueError(
            f"Missing columns: {', '.join(sorted(missing))}"
        )

    print(f"Loaded {len(df)} tickets")

    category_model = train_target(df, "category")
    priority_model = train_target(df, "priority")
    sentiment_model = train_target(df, "sentiment")

    models = {
        "category": category_model,
        "priority": priority_model,
        "sentiment": sentiment_model
    }

    joblib.dump(models, MODEL_PATH)

    print(f"\nModels saved to {MODEL_PATH}")


if __name__ == "__main__":
    main()
