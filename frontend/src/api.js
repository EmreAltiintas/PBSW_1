/**
 * Base URL for the ASP.NET API.
 * - Cross-origin (CORS): set VITE_API_BASE e.g. https://localhost:7154 (must match backend + Cors:Origins).
 * - Same-origin via Vite proxy: leave VITE_API_BASE empty and call paths like /api/...
 */
const rawBase = import.meta.env.VITE_API_BASE?.replace(/\/$/, '') ?? ''

export function apiUrl(path) {
  const p = path.startsWith('/') ? path : `/${path}`
  return rawBase ? `${rawBase}${p}` : p
}

export async function fetchJson(path, options = {}) {
  const res = await fetch(apiUrl(path), {
    ...options,
    credentials: options.credentials ?? 'include',
    headers: {
      Accept: 'application/json',
      ...options.headers,
    },
  })

  if (!res.ok) {
    const text = await res.text().catch(() => '')
    throw new Error(text || `${res.status} ${res.statusText}`)
  }

  if (res.status === 204) return null

  const ct = res.headers.get('content-type')
  if (ct?.includes('application/json')) return res.json()

  return null
}

export function getLawnMowers() {
  return fetchJson('/api/LawnMowers')
}
