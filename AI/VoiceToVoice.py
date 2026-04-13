import os
import time
import requests
from dotenv import load_dotenv
from fastapi import UploadFile
from langchain_core.runnables import RunnableLambda
from google import genai

from chatbot import ask_question as Rag_Answer

# ================== LOAD ENV ==================
load_dotenv()

GEMINI_API_KEY = os.getenv("GEMINI_API_KEY")
ELEVENLABS_API_KEY = os.getenv("ELEVENLABS_API_KEY")


# ================== SPEECH → TEXT ==================
def transcribe_audio(file: UploadFile) -> str:
    try:
        client = genai.Client(api_key=GEMINI_API_KEY)

        content = file.file.read()  # مهم جدًا

        temp_path = f"temp_{file.filename}"

        with open(temp_path, "wb") as f:
            f.write(content)

        uploaded_file = client.files.upload(file=temp_path)

        while uploaded_file.state.name == "PROCESSING":
            time.sleep(1)
            uploaded_file = client.files.get(name=uploaded_file.name)

        response = client.models.generate_content(
            model="gemini-2.5-flash",
            contents=[
                uploaded_file,
                "حول هذا الصوت إلى نص عربي فقط بدون أي شرح"
            ]
        )

        client.files.delete(name=uploaded_file.name)
        os.remove(temp_path)

        return response.text

    except Exception as e:
        print("TRANSCRIBE ERROR:", e)
        return None


# ================== RAG ANSWER ==================
def ask_question(query: str) -> str:
    try:
        return Rag_Answer(query)
    except Exception as e:
        print("RAG ERROR:", e)
        return None


# ================== TEXT → VOICE ==================
def text_to_voice(text: str) -> bytes:
    try:
        voice_id = "JBFqnCBsd6RMkjVDRZzb"
        url = f"https://api.elevenlabs.io/v1/text-to-speech/{voice_id}"

        headers = {
            "xi-api-key": ELEVENLABS_API_KEY,
            "Content-Type": "application/json",
            "Accept": "audio/mpeg"
        }

        data = {
            "text": text,
            "model_id": "eleven_multilingual_v2",
            "voice_settings": {
                "stability": 0.5,
                "similarity_boost": 0.75
            }
        }

        response = requests.post(url, headers=headers, json=data)

        if response.status_code == 200:
            return response.content

        print(response.text)
        return None

    except Exception as e:
        print("TTS ERROR:", e)
        return None


# ================== VOICE CHAIN ==================
def create_voice_chain():

    def voice_to_text_step(file: UploadFile) -> dict:
        text = transcribe_audio(file)
        return {"question": text}

    def text_to_text_step(data: dict) -> dict:
        answer = ask_question(data["question"])
        return {
            "question": data["question"],
            "answer": answer
        }

    def text_to_voice_step(data: dict) -> dict:
        audio_bytes = text_to_voice(data["answer"])
        return {
            "question": data["question"],
            "answer": data["answer"],
            "audio_bytes": audio_bytes
        }

    return (
        RunnableLambda(voice_to_text_step)
        | RunnableLambda(text_to_text_step)
        | RunnableLambda(text_to_voice_step)
    )