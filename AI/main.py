from fastapi import FastAPI, UploadFile, File
from fastapi.responses import StreamingResponse
from contextlib import asynccontextmanager
import io

from chatbot import ask_question, close_connection
from VoiceToVoice import create_voice_chain

# ================== LIFESPAN ==================
@asynccontextmanager
async def lifespan(app: FastAPI):
    print("🚀 App starting...")

    # إنشاء الـ chain مرة واحدة (مهم جدًا للأداء)
    app.state.voice_chain = create_voice_chain()

    yield

    print("🛑 App shutting down...")
    close_connection()


app = FastAPI(lifespan=lifespan)


# ================== TEXT CHAT ==================
from pydantic import BaseModel

class TextRequest(BaseModel):
    message: str


@app.post("/ask")
def ask(data: TextRequest):
    try:
        return {"response": ask_question(data.message)}
    except Exception as e:
        return {"error": str(e)}


# ================== VOICE API ==================
@app.post("/voice")
def voice(voice_file: UploadFile = File(...)):
    try:
        result = app.state.voice_chain.invoke(voice_file)

        audio_bytes = result["audio_bytes"]

        return StreamingResponse(
            io.BytesIO(audio_bytes),
            media_type="audio/mpeg",
            headers={"Content-Disposition": "inline; filename=response.mp3"}
        )

    except Exception as e:
        return {"error": str(e)}